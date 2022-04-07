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
using Microsoft.AspNet.Identity;

namespace HospitalApplication.Controllers
{
    public class PostDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all posts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all posts in the database, including their associated UserIDs.
        /// </returns>
        /// <example>
        /// GET: api/PostData/ListPosts
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PostDto))]
        public IEnumerable<PostDto> ListPosts()
        {
            List<Post> Posts = db.Posts.OrderByDescending(s => s.DateCreated).ToList();
            List<PostDto> PostDtos = new List<PostDto>();

            Posts.ForEach(a => PostDtos.Add(new PostDto()
            {
                PostID = a.PostID,
                Title = a.Title,
                DateCreated = a.DateCreated,
                Content = a.Content,
                UserID = a.UserID
            }));

            return PostDtos;
        }

        /// <summary>
        /// Returns latest 3 posts in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: latest 3 posts in the database, including their associated UserIDs.
        /// </returns>
        /// <example>
        /// GET: api/PostData/ListPostsForHome
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PostDto))]
        public IEnumerable<PostDto> ListPostsForHome()
        {
            List<Post> Posts = db.Posts.OrderByDescending(s => s.DateCreated).Take(3).ToList();
            List<PostDto> PostDtos = new List<PostDto>();

            Posts.ForEach(a => PostDtos.Add(new PostDto()
            {
                PostID = a.PostID,
                Title = a.Title,
                DateCreated = a.DateCreated,
                Content = a.Content,
                UserID = a.UserID
            }));

            return PostDtos;
        }

        /// <summary>
        /// Gathers information about all posts related to a particular user ID
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all posts in the database, including their associated user matched with a particular user ID
        /// </returns>
        /// <param name="id">User ID</param>
        /// <example>
        /// GET: api/PostData/ListPostsForUser/2928318c-4f35-43c7-9fab-660bda840719
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PostDto))]
        public IHttpActionResult ListPostsForUser(string id)
        {
            List<Post> Posts = db.Posts.Where(a => a.UserID == id).OrderByDescending(s => s.DateCreated).ToList();
            List<PostDto> PostDtos = new List<PostDto>();

            Posts.ForEach(a => PostDtos.Add(new PostDto()
            {
                PostID = a.PostID,
                Title = a.Title,
                DateCreated = a.DateCreated,
                Content = a.Content,
                UserID = a.UserID
            }));

            return Ok(PostDtos);
        }

        /// <summary>
        /// Returns a post in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A post in the system matching up to the post ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the post</param>
        /// <example>
        /// GET: api/PostData/FindPost/5
        /// </example>
        [ResponseType(typeof(PostDto))]
        [HttpGet]
        public IHttpActionResult FindPost(int id)
        {
            Post Post = db.Posts.Find(id);
            PostDto PostDto = new PostDto()
            {
                PostID = Post.PostID,
                Title = Post.Title,
                DateCreated = Post.DateCreated,
                Content = Post.Content,
                UserID = Post.UserID
            };
            if (Post == null)
            {
                return NotFound();
            }

            return Ok(PostDto);
        }

        /// <summary>
        /// Updates a particular post in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Post ID primary key</param>
        /// <param name="post">JSON FORM DATA of a post</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/PostData/UpdatePost/5
        /// FORM DATA: Post JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdatePost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.PostID)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;
            //do not modify the attached user id on update
            db.Entry(post).Property(b => b.UserID).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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
        /// Adds a post to the system
        /// </summary>
        /// <param name="post">JSON FORM DATA of a post</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Post ID, Post Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/PostData/AddPost
        /// FORM DATA: Post JSON Object
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //attach the id
            post.UserID = User.Identity.GetUserId();

            db.Posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.PostID }, post);
        }

        /// <summary>
        /// Deletes a post from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the post</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PostData/DeletePost/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Post))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.PostID == id) > 0;
        }
    }
}