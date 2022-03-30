using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models
{
    public class Donor
    {
        public int DonorID { get; set; }
        public string DonorName { get; set; }
        public string DonorEmail { get; set; }
        public string DonorAddress { get; set; }
        public string DonorPhone { get; set; }
    }
}