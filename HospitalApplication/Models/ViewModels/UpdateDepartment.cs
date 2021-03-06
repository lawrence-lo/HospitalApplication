using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class UpdateDepartment
    {
        //existing department information
        public DepartmentDto SelectedDepartment { get; set; }

        //all employees to choose from when updating this department
        public IEnumerable<UserDto> DepartmentUsers { get; set; }
    }
}