using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class DetailsDepartment
    {
        public DepartmentDto SelectedDepartment { get; set; }
        public IEnumerable<UserDto> DepartmentUsers { get; set; }
        public IEnumerable<JobDto> DepartmentJobs { get; set; }
    }
}