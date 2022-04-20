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
    public class DonorDataController : ApiController
    {
      
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: api/DonorData/ListDonors
        [HttpGet]
        [ResponseType(typeof(DonorDto))]

        public IHttpActionResult ListDonors()
        {
            List<Donor> Donors = db.Donors.ToList();
            List<DonorDto> DonorDtos = new List<DonorDto>();

            Donors.ForEach(d => DonorDtos.Add(new DonorDto()
            {
                DonorID = d.DonorID,
                DonorName = d.DonorName,
                DonorAddress = d.DonorAddress,
                DonorEmail = d.DonorEmail,
                DonorPhone = d.DonorPhone
            }));
            return Ok(DonorDtos);
        }

        // GET: api/DonorData/FindDonor/5
        [HttpGet]
        [ResponseType(typeof(DonorDto))]
        public IHttpActionResult FindDonor(int id)
        {
            Donor Donor = db.Donors.Find(id);
            DonorDto DonorDto = new DonorDto()
            {
                DonorID = Donor.DonorID,
                DonorName = Donor.DonorName,
                DonorAddress = Donor.DonorAddress,
                DonorEmail = Donor.DonorEmail,
                DonorPhone = Donor.DonorPhone,
                
            };
            if (Donor == null)
            {
                return NotFound();
            }

            return Ok(DonorDto);
        }

        // POST: api/DonorData/UpdateDonor/5
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult UpdateDonor(int id, Donor donor)
        {
            Debug.WriteLine("I have reached API");
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != donor.DonorID)
            {
                return BadRequest();
            }
            Debug.WriteLine("id is" + id);
            Debug.WriteLine("DonorID is" + donor.DonorID);
            db.Entry(donor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonorExists(id))
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

        // POST: api/DonorData/AddDonor
        [ResponseType(typeof(Donor))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult AddDonor(Donor donor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Donors.Add(donor);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = donor.DonorID }, donor);
        }

        // POST: api/DonorData/DeleteDonor/5
        [ResponseType(typeof(Donor))]
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public IHttpActionResult DeleteDonor(int id)
        {
            Donor donor = db.Donors.Find(id);
            if (donor == null)
            {
                return NotFound();
            }

            db.Donors.Remove(donor);
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

        private bool DonorExists(int id)
        {
            return db.Donors.Count(e => e.DonorID == id) > 0;
        }
    }
}