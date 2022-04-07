﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class NewDonation
    {
        public DonationDto DonationDto { get; set; }

        //list of Donors
        public IEnumerable<DonorDto> DonorOptions { get; set; }
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
    }
}