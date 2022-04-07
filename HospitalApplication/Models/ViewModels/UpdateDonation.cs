using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalApplication.Models.ViewModels
{
    public class UpdateDonation
    {
        public DonationDto SelectedDonation { get; set; }

        //list of Donors
        public IEnumerable<DonorDto> DonorOptions { get; set; }
        public IEnumerable<DepartmentDto> DepartmentOptions { get; set; }
    }
}