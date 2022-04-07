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
    public class HospitalAppointmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Patient Appointments in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Hospital Appointments in the database
        /// </returns>
        /// <example>
        /// </example>
        // GET: api/HospitalAppointmentData/ListHospitalAppointments

        [ResponseType(typeof(HospitalAppointmentDto))]
        [HttpGet]
        public IHttpActionResult ListHospitalAppointments()
        {
            List<HospitalAppointment> hospitalAppointments = db.HospitalAppointments.ToList();
            List<HospitalAppointmentDto> hospitalAppointmentDtos = new List<HospitalAppointmentDto>();

            hospitalAppointments.ForEach(a => hospitalAppointmentDtos.Add(new HospitalAppointmentDto()
            {
                AppointmentID = a.AppointmentID,
                DateCreated = a.DateCreated,
                DoctorName = a.DoctorName,
                Description = a.Description,
                PatientID = a.PatientID.ToString(),
                PatientName = a.HospitalPatient.Name,
                UserID = a.UserID
 
            }));
            return Ok(hospitalAppointmentDtos);
        }

        /// <summary>
        /// Returns a PatientAppointment mapping to the given ID in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An appointmebt in the system matching up to the Appointment ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Appointment</param>
        /// <example>
        /// GET: api/HospitalAppointmentData/FindAppointment/5
        /// </example>

        [ResponseType(typeof(HospitalAppointmentDto))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            HospitalAppointment hospitalAppointment = db.HospitalAppointments.Find(id);
            HospitalAppointmentDto hospitalAppointmentDto = new HospitalAppointmentDto()
            {
                AppointmentID = hospitalAppointment.AppointmentID,
                DateCreated = hospitalAppointment.DateCreated,
                DoctorName = hospitalAppointment.DoctorName,
                Description = hospitalAppointment.Description,
                PatientID = hospitalAppointment.PatientID.ToString(),
                PatientName = hospitalAppointment.HospitalPatient.Name,
                UserID = hospitalAppointment.UserID
            };

            if (hospitalAppointment == null)
            {
                return NotFound();
            }

            return Ok(hospitalAppointmentDto);
        }


        /// <summary>
        /// Updates a particular Appointment in the system with POST Data Input
        /// </summary>
        /// <param name="id">Represents the appointmentID Primary Key</param>
        /// <param name="hospitalAppointment">JSON Form Data of a Appointment including id of appointment to be updated</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response) or
        /// HEADER: 400 (Not Found)
        /// </returns>
        /// <example>
        /// POST: PUT: api/HospitalAppointmentData/updateHospitalAppointment/5
        /// hospitalAppointment Json Object
        /// </example>

        [Route("api/HospitalAppointmentData/updateHospitalAppointment/{id}")]
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]

        public IHttpActionResult UpdateHospitalAppointment(int id, HospitalAppointment hospitalAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hospitalAppointment.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(hospitalAppointment).State = EntityState.Modified;

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
        /// Add a new Appointment to the system
        /// </summary>
        /// <param name="drug">JSON form data of a new appointment (no id)</param> 
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Apoointment Id, Appointment Data
        /// or
        /// HEADER: 404 (Bad request)
        /// </returns>
        /// <example>
        /// POST: api/HospitalAppointmentData/addNewAppointment
        /// FORM DATA: Appointment json object
        /// </example>

        [ResponseType(typeof(HospitalAppointment))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddNewAppointment(HospitalAppointment hospitalAppointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.HospitalAppointments.Add(hospitalAppointment);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = hospitalAppointment.AppointmentID }, hospitalAppointment);
        }


        /// <summary>
        /// Deletes an Appointment from the system matching to the given Appointment Id
        /// </summary>
        /// <param name="id">The primary key of the Appointment</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        ///  DELETE: api/HospitalAppointmentData/deleteHospitalAppointment/5
        /// </example>

        [ResponseType(typeof(HospitalAppointment))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteHospitalAppointment(int id)
        {
            HospitalAppointment hospitalAppointment = db.HospitalAppointments.Find(id);
            if (hospitalAppointment == null)
            {
                return NotFound();
            }

            db.HospitalAppointments.Remove(hospitalAppointment);
            db.SaveChanges();

            return Ok(hospitalAppointment);
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

