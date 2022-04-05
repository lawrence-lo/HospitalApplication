﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }
        public DateTime DateCreated { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }

        //each appointment entity points to one patient entity

        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        //each appoinment entity is created by or pointing to one user/employee
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public DateTime DateCreated { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }
        public string PatientID { get; set; }
        public string UserID { get; set; }
    }


}