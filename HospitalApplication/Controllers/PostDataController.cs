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
//using System.Diagnostics;

namespace HospitalApplication.Controllers
{
    public class PostDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PostData/ListPosts
        [HttpGet]
        public IEnumerable<PostDto> ListPosts()
        {
            List<Post> Posts = db.Posts.ToList();
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

        // GET: api/PostData/FindPost/5
        [ResponseType(typeof(Post))]
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

        // POST: api/PostData/UpdatePost/5
        [ResponseType(typeof(void))]
        [HttpPost]
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

        // POST: api/PostData/AddPost
        [ResponseType(typeof(Post))]
        [HttpPost]
        public IHttpActionResult AddPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.PostID }, post);
        }

        // POST: api/PostData/DeletePost/5
        [ResponseType(typeof(Post))]
        [HttpPost]
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