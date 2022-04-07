using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class DetailsAppointment
    {
        public HospitalPatientDto SelectedAppointment { get; set; }
        public IEnumerable<HospitalPatientDto> Patients { get; set; }
    }
}