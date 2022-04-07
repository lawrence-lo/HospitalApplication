using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class DetailsPatient
    {
        public HospitalPatientDto SelectedPatient { get; set; }
        public IEnumerable<HospitalAppointmentDto> RelatedAppointments { get; set; }
    }
}