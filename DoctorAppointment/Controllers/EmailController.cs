using DoctorAppointment.Data;
using DoctorAppointment.Models;
using DoctorAppointment.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DoctorAppointment.Controllers
{
    public class EmailController : Controller
    {
        private readonly IMailService _service;
        private readonly MyContext _context;

        /// <summary>
        ///     dependency injection 
        /// </summary>
        /// <param></param>
        public EmailController(IMailService service, MyContext context)
        {
            _context = context;
            _service = service;
        }
        public IActionResult Send()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Send(MailContent mail)
        {
            _service.sendEmail(mail);
            return View();
        }
        public async Task<IActionResult> Approve(int id)
        {
            // Get appointment details by ID
            var appointment = await _context.tblAppointment.SingleOrDefaultAsync(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return NotFound();
            }
            
            // Redirect to email form with patient email address pre-populated
            return RedirectToAction("Send", new { toEmailAddress = appointment.PatientEmail });
        }
    }
}
