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
    public class HospitalPatientDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Patients in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Patients in the database
        /// </returns>
        /// <example>
        /// </example>
        // GET: api/HospitalPatientData/ListHospitalPatients

        [ResponseType(typeof(HospitalPatientDto))]
        [HttpGet]
        public IHttpActionResult ListHospitalPatients()
        {
            List<HospitalPatient> hospitalPatients = db.HospitalPatients.ToList();
            List<HospitalPatientDto> hospitalPatientDtos = new List<HospitalPatientDto>();

            hospitalPatients.ForEach(p => hospitalPatientDtos.Add(new HospitalPatientDto()
            {
                PatientID = p.PatientID,
                Name = p.Name,
                DOB = p.DOB,
                PhoneNo = p.PhoneNo,
                IsActive = p.IsActive
            }));
            return Ok(hospitalPatientDtos);
        }

        /// <summary>
        /// Returns a Patient mapping given ID in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Patient in the system matching up to the Patient ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Patient</param>
        /// <example>
        /// GET: api/HospitalPatientData/FindPatient/5
        /// </example>
       
        [ResponseType(typeof(HospitalPatientDto))]
        [HttpGet]
        public IHttpActionResult FindPatient(int id)
        {
            HospitalPatient hospitalPatient = db.HospitalPatients.Find(id);
            HospitalPatientDto hospitalPatientDto = new HospitalPatientDto()
            {
                PatientID = hospitalPatient.PatientID,
                Name = hospitalPatient.Name,
                DOB = hospitalPatient.DOB,
                PhoneNo = hospitalPatient.PhoneNo,
                IsActive = hospitalPatient.IsActive
            };

            if (hospitalPatient == null)
            {
                return NotFound();
            }

            return Ok(hospitalPatientDto);
        }


        /// <summary>
        /// Updates a particular patient in the system with POST Data Input
        /// </summary>
        /// <param name="id">Represents the patientID Primary Key</param>
        /// <param name="drug">JSON Form Data of a Patient including id of patient to be updated</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response) or
        /// HEADER: 400 (Not Found)
        /// </returns>
        /// <example>
        /// POST: PUT: api/HospitalPatientData/updatePatient/5
        /// Drug Json Object
        /// </example>

        [Route("api/HospitalPatientData/updatePatient/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateHospitalPatient(int id, HospitalPatient hospitalPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hospitalPatient.PatientID)
            {
                return BadRequest();
            }

            db.Entry(hospitalPatient).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HospitalPatientExists(id))
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



        /// <summary>
        /// Add a new patient to the system
        /// </summary>
        /// <param name="drug">JSON form data of a new patient (no id)</param> 
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: patient Id, patient Data
        /// or
        /// HEADER: 404 (Bad request)
        /// </returns>
        /// <example>
        /// POST: api/HospitalPatientData/addNewPatient
        /// FORM DATA: Patient json object
        /// </example>

        [ResponseType(typeof(HospitalPatient))]
        [HttpPost]
        public IHttpActionResult AddNewPatient(HospitalPatient hospitalPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HospitalPatients.Add(hospitalPatient);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hospitalPatient.PatientID }, hospitalPatient);
        }


        /// <summary>
        /// Deletes a Patient from the system matching to the given Patient Id
        /// </summary>
        /// <param name="id">The primary key of the Patient</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///  DELETE: api/HospitalPatientData/deletePatient/5
        /// </example>
      
        [ResponseType(typeof(HospitalPatient))]
        public IHttpActionResult DeleteHospitalPatient(int id)
        {
            HospitalPatient hospitalPatient = db.HospitalPatients.Find(id);
            if (hospitalPatient == null)
            {
                return NotFound();
            }

            db.HospitalPatients.Remove(hospitalPatient);
            db.SaveChanges();

            return Ok(hospitalPatient);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HospitalPatientExists(int id)
        {
            return db.HospitalPatients.Count(e => e.PatientID == id) > 0;
        }
    }
}