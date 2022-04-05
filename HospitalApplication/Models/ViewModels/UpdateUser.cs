using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class UpdateUser
    {
        //the existing user information
        public UserDto SelectedUser { get; set; }

        //list of departments
        public IEnumerable<DepartmentDto> DepartmentsOptions { get; set; }
    }
}