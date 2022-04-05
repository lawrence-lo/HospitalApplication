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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        public IEnumerable<DepartmentDto> ListDepartments()
        {
            List<Department> Departments = db.Departments.ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DeptID = d.DeptID,
                DeptName = d.DeptName,
                DeptLocation = d.DeptLocation,
                DeptDescription = d.DeptDescription
            }));
            return DepartmentDtos;
        }

        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartmentsForDonation(int id)
        {
            List<Department> Departments = db.Departments.Where(
                d => d.Donations.Any(
                    o => o.DonationID == id
                )).ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DeptID = d.DeptID,
                DeptName = d.DeptName,
                DeptLocation = d.DeptLocation,
                DeptDescription = d.DeptDescription
            }));

            return Ok(DepartmentDtos);
        }

        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartmentsForUser(int id)
        {
            List<Department> Departments = db.Departments.Where(
                d => d.Users.Any(
                    u => u.UserID == id
                )).ToList();
            List<DepartmentDto> DepartmentDtos = new List<DepartmentDto>();

            Departments.ForEach(d => DepartmentDtos.Add(new DepartmentDto()
            {
                DeptID = d.DeptID,
                DeptName = d.DeptName,
                DeptLocation = d.DeptLocation,
                DeptDescription = d.DeptDescription
            }));

            return Ok(DepartmentDtos);
        }

        // GET: api/DepartmentData/FindDepartment/5
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department Department = db.Departments.Find(id);
            DepartmentDto DepartmentDto = new DepartmentDto()
            {
                DeptID = Department.DeptID,
                DeptName = Department.DeptName,
                DeptLocation = Department.DeptLocation,
                DeptDescription = Department.DeptDescription
            };
            if (Department == null)
            {
                return NotFound();
            }

            return Ok(DepartmentDto);
        }

        // POST: api/DepartmentData/UpdateDepartment/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DeptID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/DepartmentData/AddDepartment
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DeptID }, department);
        }

        // POST: api/DepartmentData/DeleteDepartment/5
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
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

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DeptID == id) > 0;
        }
    }
}