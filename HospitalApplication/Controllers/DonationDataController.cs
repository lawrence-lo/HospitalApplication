using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalApplication.Models;
using System.Diagnostics;

namespace HospitalApplication.Controllers
{
    public class DonationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonationData/ListDonations
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult ListDonations()
        {
            List<Donation> Donations = db.Donations.ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationID = d.DonationID,
                DonationDescription = d.DonationDescription,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonorID=d.Donor.DonorID,
                DonorName= d.Donor.DonorName,
                DeptID=d.Department.DeptID,
                DeptName=d.Department.DeptName
            }));
            return Ok(DonationDtos);
        }

        /// <summary>
        /// Gather information about all donations related to a donor
        /// </summary>
        /// <returns>
        /// All Donations in the databas, including thier associated Donor
        /// </returns>
        /// <param name="id">Donor ID.</param>

        /// GET: api/DonationData/ListDonationsForDonor/3
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult ListDonationsForDonor(int id)
        {
            List<Donation> Donations = db.Donations.Where(d=>d.DonorID==id).ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationID = d.DonationID,
                DonationDescription = d.DonationDescription,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonorID = d.Donor.DonorID,
                DonorName = d.Donor.DonorName,
                DeptID = d.Department.DeptID,
                DeptName = d.Department.DeptName
            }));
            return Ok(DonationDtos);
        }
        /// <summary>
        /// Gather information about all donations related to a department
        /// </summary>
        /// <returns>
        /// All Donations in the databas, including the associated Department
        /// </returns>
        /// <param name="id">Department ID.</param>

        /// GET: api/DonationData/ListDonationsForDepartment/3
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult ListDonationsForDepartment(int id)
        {
            //all donation to a department which matches the ID
            List<Donation> Donations = db.Donations.Where(d => d.DeptID == id).ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationID = d.DonationID,
                DonationDescription = d.DonationDescription,
                DonationDate = d.DonationDate,
                DonationAmount = d.DonationAmount,
                DonorID = d.Donor.DonorID,
                DonorName = d.Donor.DonorName,
                DeptID = d.Department.DeptID,
                DeptName = d.Department.DeptName
            }));
            return Ok(DonationDtos);
        }

        // GET: api/DonationData/FindDonation/5
        [HttpGet]
        [ResponseType(typeof(DonationDto))]
        public IHttpActionResult FindDonation(int id)
        {
            Donation Donation = db.Donations.Find(id);
            DonationDto DonationDto = new DonationDto()
            {
                DonationID = Donation.DonationID,
                DonationDescription = Donation.DonationDescription,
                DonationDate = Donation.DonationDate,
                DonationAmount = Donation.DonationAmount,
                DonorID = Donation.Donor.DonorID,
                DonorName = Donation.Donor.DonorName,
                DeptID = Donation.Department.DeptID,
                DeptName = Donation.Department.DeptName

            };
            if (Donation == null)
            {
                return NotFound();
            }

            return Ok(DonationDto);
        }

        // POST: api/DonationData/UpdateDonation/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult UpdateDonation(int id, Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donation.DonationID)
            {
                return BadRequest();
            }

            db.Entry(donation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DonationData/AddDonation
        [ResponseType(typeof(Donation))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult AddDonation(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donations.Add(donation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donation.DonationID }, donation);
        }

        // POST: api/DonationData/DeleteDonation/5
        [ResponseType(typeof(Donation))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            Debug.WriteLine("I have reached API" + id);
            db.Donations.Remove(donation);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DonationExists(int id)
        {
            return db.Donations.Count(e => e.DonationID == id) > 0;
        }
    }
}