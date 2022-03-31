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

namespace HospitalApplication.Controllers
{
    public class DonationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DonationData/ListDonations
        [HttpGet]
        public IEnumerable<DonationDto> ListDonations()
        {
            List<Donation> Donations = db.Donations.ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            Donations.ForEach(d => DonationDtos.Add(new DonationDto()
            {
                DonationID = d.DonationID,
                DonationAmount = d.DonationAmount,
                DonationDate = d.DonationDate,
                DonationDescription = d.DonationDescription,
                DonorID = d.Donor.DonorID,
                DonorName = d.Donor.DonorName
            }));
            return DonationDtos;
        }

        // GET: api/DonationData/FindDonation/5
        
        [ResponseType(typeof(Donation))]
        [HttpGet]
        public IHttpActionResult FindDonation(int id)
        {
            Donation Donation = db.Donations.Find(id);
            DonationDto DonationDto = new DonationDto()
            {
                DonationID = Donation.DonationID,
                DonationAmount = Donation.DonationAmount,
                DonationDate = Donation.DonationDate,
                DonationDescription = Donation.DonationDescription,
                DonorID = Donation.Donor.DonorID,
                DonorName = Donation.Donor.DonorName
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
        public IHttpActionResult DeleteDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }

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