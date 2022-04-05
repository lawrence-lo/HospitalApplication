using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class UpdateJob
    {
        //existing job information
        public JobDto SelectedJob { get; set; }

        //all departments to choose from when updating this job
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
    }
}