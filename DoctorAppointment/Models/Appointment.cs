using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorAppointment.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public string AppointmentDate { get; set; } 
        public int PatID { get; set; }
        public int DocID { get; set; }
        public bool Approved { get; internal set; }
        public string AppointmentDay { get; set; }
        [NotMapped]
        public PatientInfo Patient { get; internal set; }
        [NotMapped]
        public string PatientName { get; internal set; }
        [NotMapped]
        public object DoctorName { get; internal set; }
        [NotMapped]
        public string PatientName2 { get; internal set; }
        [NotMapped]
        public string DoctorName2 { get; internal set; }
        [NotMapped]
        public string PatientEmail { get; internal set; }

    }
}
