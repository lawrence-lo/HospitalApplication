using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Department
    {
        [Key] 
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public string DeptLocation { get; set; }
        public string DeptDescription { get; set; }

        /* To-do: Dept Head, Staff No. */

        //A department can receive many donations
        //A department can have many jobs
        public ICollection<Donation> Donations { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }

    public class DepartmentDto
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public string DeptLocation { get; set; }
        public string DeptDescription { get; set; }
    }
}