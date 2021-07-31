using Hospital_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Hospital_Project.Controllers
{
    public class BlogDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/BlogData/ListBlogs
        [HttpGet]
        [ResponseType(typeof(BlogDto))]
        public IEnumerable<BlogDto> ListBlogs()
        {
            List<Blog> Blogs = db.Blogs.ToList();
            List<BlogDto> BlogDtos = new List<BlogDto>();
            Blogs.ForEach(e => BlogDtos.Add(new BlogDto()
            {
                BlogID = e.BlogID,
                Title = e.Title,
                Content = e.Content
            }));
            return BlogDtos;
        }


        // GET: api/BlogData/FindBlog/5
        [HttpGet]
        [ResponseType(typeof(BlogDto))]
        public IHttpActionResult FindBlog(int id)
        {
            Blog blog = db.Blogs.Find(id);
            BlogDto blogDto = new BlogDto()
            {
                BlogID = blog.BlogID,
                Title = blog.Title,
                Content = blog.Content
            };
            if (blog == null)
            {
                return NotFound();
            }

            return Ok(blogDto);
        }

        // PUT: api/BlogData/UpdateBlog/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateBlog(int id, Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != blog.BlogID)
            {
                return BadRequest();
            }

            db.Entry(blog).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
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

        // POST: api/BlogData/AddBlog
        [ResponseType(typeof(Blog))]
        public IHttpActionResult AddBlog(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Blogs.Add(blog);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = blog.BlogID }, blog);
        }

        // DELETE: api/BlogData/5
        [ResponseType(typeof(Blog))]
        public IHttpActionResult DeleteBlog(int id)
        {
            Blog blog = db.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }

            db.Blogs.Remove(blog);
            db.SaveChanges();

            return Ok(blog);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BlogExists(int id)
        {
            return db.Blogs.Count(e => e.BlogID == id) > 0;
        }
    }
}
