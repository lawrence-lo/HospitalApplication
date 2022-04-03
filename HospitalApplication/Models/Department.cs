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
        public ICollection<Donation> Donations { get; set; }
    }
}