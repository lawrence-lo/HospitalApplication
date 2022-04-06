using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class HospitalPatient
    {
        [Key]
        public int PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNo { get; set; }
        public Boolean IsActive { get; set; }

        // One to many relationship between Patient and Appointment
        //A patient can have zero, one or many appointments
        //An appointment can belong to only one patient
        public ICollection<HospitalAppointment> HospitalAppointments { get; set; }
    }

    public class HospitalPatientDto
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public DateTime DOB { get; set; }
        public string PhoneNo { get; set; }
        public Boolean IsActive { get; set; }

    }
}