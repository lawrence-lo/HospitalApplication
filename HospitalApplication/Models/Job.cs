using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models
{
    public class Job
    {
        [Key]
        public int JobID { get; set; }
        public string JobName { get; set; }
        public int JobSalary { get; set; }
        public string JobHour { get; set; }
        public string JobDescription { get; set; }

        //A job belongs to one department
        //A department can have many jobs
        [ForeignKey("Department")]
        public int DeptID { get; set; }
        public virtual Department Department { get; set; }
    }

    public class JobDto
    {
        public int JobID { get; set; }
        public string JobName { get; set; }
        public int JobSalary { get; set; }
        public string JobHour { get; set; }
        public string JobDescription { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
    }
}