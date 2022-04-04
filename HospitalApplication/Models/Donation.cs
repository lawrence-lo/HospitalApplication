using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalApplication.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationDescription { get; set; }


        [ForeignKey("Donor")]
        public int DonorID { get; set; }
        public virtual Donor Donor { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Department")]
        public int DeptID { get; set; }
        public virtual Department Department { get; set; }

    }

    public class DonationDto
    {
        public int DonationID { get; set; } 
        public decimal DonationAmount { get; set; }
        public DateTime DonationDate { get; set; }
        public string DonationDescription { get; set; }
        
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        
        public string UserID { get; set; }
        
        public int DeptID { get; set; }
        public string DeptName { get; set; }
    }
}