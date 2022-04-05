using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using HospitalApplication.Models;

namespace HospitalApplication.Controllers
{
    public class UserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all users in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all users in the database, including their associated species.
        /// </returns>
        /// <example>
        /// GET: api/UserData/ListUsers
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        [Authorize]
        public IHttpActionResult ListUsers()
        {
            List<ApplicationUser> Users = db.Users.ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(a => UserDtos.Add(new UserDto()
            {
                UserID = a.Id,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                UserName = a.UserName,
                LastName = a.LastName,
                GivenName = a.GivenName,
                DateOfBirth = a.DateOfBirth,
                Department = a.Department,
                Position = a.Position,
                HireDate = a.HireDate,
                Salary = a.Salary
            }));

            return Ok(UserDtos);
        }

        /// <summary>
        /// Returns all users in the system associated with a particular department.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all users in the database under the same department
        /// </returns>
        /// <param name="id">Department Primary Key</param>
        /// <example>
        /// GET: api/UserData/ListUsersForDepartment/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult ListUsersForDepartment(int id)
        {
            List<ApplicationUser> Users = db.Users.Where(a => a.Department == id).ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(a => UserDtos.Add(new UserDto()
            {
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                LastName = a.LastName,
                GivenName = a.GivenName,
                Department = a.Department,
                Position = a.Position,
            }));

            return Ok(UserDtos);
        }

        /// <summary>
        /// Returns all doctors in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all doctors in the database
        /// </returns>
        /// <example>
        /// GET: api/UserData/ListDoctors
        /// </example>
        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public IHttpActionResult ListDoctors()
        {
            List<ApplicationUser> Users = db.Users.Where(a => a.Position.ToLower().Contains("doctor")).ToList();
            List<UserDto> UserDtos = new List<UserDto>();

            Users.ForEach(a => UserDtos.Add(new UserDto()
            {
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                LastName = a.LastName,
                GivenName = a.GivenName,
                Department = a.Department,
                Position = a.Position,
            }));

            return Ok(UserDtos);
        }

        /// <summary>
        /// Returns a user in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A user in the system matching up to the userID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the user</param>
        /// <example>
        /// GET: api/UserData/FindUser/5
        /// </example>
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(ApplicationUser))]
        public IHttpActionResult FindUser(string id)
        {
            ApplicationUser User = db.Users.Find(id);
            UserDto UserDto = new UserDto()
            {
                UserID = User.Id,
                Email = User.Email,
                PhoneNumber = User.PhoneNumber,
                UserName = User.UserName,
                LastName = User.LastName,
                GivenName = User.GivenName,
                DateOfBirth = User.DateOfBirth,
                Department = User.Department,
                Position = User.Position,
                HireDate = User.HireDate,
                Salary = User.Salary
            };
            if (User == null)
            {
                return NotFound();
            }

            return Ok(UserDto);
        }

        /// <summary>
        /// Returns the full name of a user in the system for post details.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A full name of a user in the system matching up to the userID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the user</param>
        /// <example>
        /// GET: api/UserData/FindUserForPost/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult FindUserForPost(string id)
        {
            ApplicationUser User = db.Users.Find(id);
            string FullName = User.GivenName + " " + User.LastName;

            if (User == null)
            {
                return NotFound();
            }

            return Ok(FullName);
        }

        /// <summary>
        /// Updates a particular user in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the user ID primary key</param>
        /// <param name="applicationUser">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/UserData/UpdateUser/5
        /// FORM DATA: User JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateUser(string id, ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            db.Entry(applicationUser).State = EntityState.Modified;
            // Prevent updating other fields
            db.Entry(applicationUser).Property("EmailConfirmed").IsModified = false;
            db.Entry(applicationUser).Property("PasswordHash").IsModified = false;
            db.Entry(applicationUser).Property("SecurityStamp").IsModified = false;
            db.Entry(applicationUser).Property("PhoneNumberConfirmed").IsModified = false;
            db.Entry(applicationUser).Property("TwoFactorEnabled").IsModified = false;
            db.Entry(applicationUser).Property("LockoutEndDateUtc").IsModified = false;
            db.Entry(applicationUser).Property("LockoutEnabled").IsModified = false;
            db.Entry(applicationUser).Property("AccessFailedCount").IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
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
        /// Adds a user to the system
        /// </summary>
        /// <param name="applicationUser">JSON FORM DATA of a user</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: User ID, User Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/UserData/AddUser
        /// FORM DATA: Animal JSON Object
        /// </example>
        [ResponseType(typeof(ApplicationUser))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddUser(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(applicationUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserExists(applicationUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = applicationUser.Id }, applicationUser);
        }

        /// <summary>
        /// Delete a user from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the user</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/UserData/DeleteUser/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(ApplicationUser))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteUser(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            // Remove related posts
            foreach (var post in db.Posts.Where(a => a.UserID == id))
            {
                db.Posts.Remove(post);
            }
            db.SaveChanges();

            db.Users.Remove(applicationUser);
            db.SaveChanges();

            return Ok(applicationUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationUserExists(string id)
        {
            return db.Users.Count(e => e.Id == id) > 0;
        }
    }
}