using DoctorAppointment.Data;
using DoctorAppointment.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Web;
using PagedList;
using System.Threading.Tasks;
using DoctorAppointment.Services;

namespace DoctorAppointment.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly MyContext _context;
        private readonly IMailService _service;
        public AppointmentController(MyContext context, IMailService service) 
        {
            _service = service;
            _context = context;

        }
        public IActionResult BookAppointment()
        {
            var departments =   _context.tblDepartments.Select(d => d.DepartmentName).Distinct().ToList();
            ViewBag.Departments = departments;
            return View();
        }
        
        public IActionResult DepartmentbyID(int DepartId)
        {
            return View(_context.tblDoctor.Where(a => a.DepartmentId == DepartId).ToList());
        }

        [HttpPost]
        public IActionResult Add(Appointment ap)
        {
            _context.tblAppointment.Add(ap);
            _context.SaveChanges();
            return RedirectToAction("BookAppointment", "Appointment");
           // return View(_context.tblDoctor.Where(a => a.DepartmentId == DepartId).ToList());
        }

        public IActionResult Check(string department)
        {
            // Retrieve the list of appointments from your data source,
            // filtered by the selected department (if any)

            //  IQueryable<DocotorInfo> appointments = _context.tblDoctor;
            //if (!string.IsNullOrEmpty(department))
            //{
            //   // appointments = appointments.Where(a => a.Department == department);
            //}

            // Retrieve the department object for the selected department
            var selectedDepartment = _context.tblDepartments
                .Include(d => d.Doctors)
                .FirstOrDefault(d => d.DepartmentName == department);

            // Retrieve the list of doctors for the selected department
            var doctors = selectedDepartment?.Doctors?.ToList();

            // Create a view model to pass the appointments and department
            // information to the view
            //var viewModel = new AppointmentViewModel
            //{
            //    Appointments = doctors.ToList(),
            //    Department = department,
            //    Doctors = doctors
            //};

            // Pass the view model to the view
            // return View(viewModel);
            return View(doctors);
       }

        public IActionResult Create()
        {
           // var departments = _context.tblDepartments.Select(d => d.DepartmentName).Distinct().ToList();
           // ViewBag.Departments = departments;
            return View();
        }

        public IActionResult BookApppointmentByPatient()
        {
          var patID=   HttpContext.Session.GetString("Patient");
           // dateofweek
           // HttpContext.Session.GetString("dateofweek");
            HttpContext.Session.SetString("PtID", patID); ;
            // var Getrole = Context.Session.GetString("Patient");
            // var departments = _context.tblDepartments.Select(d => d.DepartmentName).Distinct().ToList();
            // ViewBag.Departments = departments;
            return View();
        }
        public async Task<IActionResult> ViewAppointments(int? pageNumber)
        {
            int pageSize = 6;
            
            // Check if the current user is a doctor
            if (User.IsInRole("Doctor"))
            {
                // Get the ID of the currently logged-in doctor
                var doctorId = HttpContext.Session.GetInt32("Doctor");

                // Retrieve the list of appointments for the doctor and join with the patients and doctors
                var appointments = _context.tblAppointment
                    .Where(a => a.DocID == doctorId)
                    .Join(_context.tblPatients,
                          a => a.PatID,
                          p => p.PatientId,
                          (a, p) => new { Appointment = a, Patient = p })
                    .Join(_context.tblDoctor,
                          a => a.Appointment.DocID,
                          d => d.DoctorId,
                          (a, d) => new { Appointment = a.Appointment, Patient = a.Patient, Doctor = d })
                    .Select(ap => new Appointment
                    {
                        AppointmentID = ap.Appointment.AppointmentID,
                        PatID = ap.Patient.PatientId,
                        PatientName = ap.Patient.First_Name,
                        PatientName2 = ap.Patient.Last_Name,
                        DoctorName = ap.Doctor.First_Name,
                        DoctorName2 = ap.Doctor.Last_Name,
                        PatientEmail = ap.Patient.EmailAddress,
                        AppointmentDate = ap.Appointment.AppointmentDate,
                        AppointmentDay = ap.Appointment.AppointmentDay,
                        Approved= ap.Appointment.Approved,
                        // add other appointment properties as needed
                    });

                return View(await PaginatedList<Appointment>.CreateAsync(appointments.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
            else
            {
                // For non-doctor users, show all appointments
                var appointments = _context.tblAppointment
                    .Join(_context.tblPatients,
                          a => a.PatID,
                          p => p.PatientId,
                          (a, p) => new { Appointment = a, Patient = p })
                    .Join(_context.tblDoctor,
                          a => a.Appointment.DocID,
                          d => d.DoctorId,
                          (a, d) => new { Appointment = a.Appointment, Patient = a.Patient, Doctor = d })
                    .Select(ap => new Appointment
                    {
                        AppointmentID = ap.Appointment.AppointmentID,
                        PatID = ap.Patient.PatientId,
                        PatientName = ap.Patient.First_Name,
                        PatientName2 = ap.Patient.Last_Name,
                        DoctorName = ap.Doctor.First_Name,
                        PatientEmail = ap.Patient.EmailAddress,
                        DoctorName2 = ap.Doctor.Last_Name,
                        AppointmentDate = ap.Appointment.AppointmentDate,
                        AppointmentDay = ap.Appointment.AppointmentDay,
                        Approved = ap.Appointment.Approved,
                        // add other appointment properties as needed
                    });

                return View(await PaginatedList<Appointment>.CreateAsync(appointments.AsNoTracking(), pageNumber ?? 1, pageSize));
            }
        }
        public async Task<IActionResult> Approved(int id)
        {
            var appointment = await _context.tblAppointment.SingleOrDefaultAsync(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Approved = true;
            await _context.SaveChangesAsync();

            var mailContent = new MailContent
            {
                ToEmailAddress = "mpanoaki@gmail.com",
                EmailSubject = "Appointment Approved",
                EmailBody = $"Thank you for booking a doctor appointment {appointment.DoctorName} {appointment.DoctorName2} on {appointment.AppointmentDate}   "
            };

            _service.sendEmail(mailContent);
            
            return Json(new { success = true });

        }

    }
}



