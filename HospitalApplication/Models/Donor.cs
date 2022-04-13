using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HospitalApplication.Models
{
    public class Donor
    {
        [Key]
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorAddress { get; set; }
        public string DonorPhone { get; set; }

        public ICollection<Donation> Donations { get; set; }


    }
    public class DonorDto
    {
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorAddress { get; set; }
        public string DonorPhone { get; set; }
        

    }
}