using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HospitalApplication.Models
{
    public class UserDto
    {
        [Key]
        public string UserID { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Department { get; set; }
        public string Position { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}