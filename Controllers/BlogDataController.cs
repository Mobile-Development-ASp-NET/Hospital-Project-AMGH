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

        /// Objective: Create a method that returns all list of blogs from the database
        /// <summary>
        /// return all blogs from the database
        /// </summary>
        /// <returns>List of blogs </returns>
        /// <example>GET: api/BlogData/ListBlogs</example>
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

        ///Objective: Create a method that allow us to return all blogs that are related to the subscribed user
        ///by entering a interger value of the selected SubscribedUser id
        /// <summary>
        /// Return all blogs that are related to the selected subscribed user from the database
        /// </summary>
        /// <param name="id">SubscribedUserID</param>
        ///<example>GET: api/BlogData/ListBlogsForSubscribedUser</example>
        [HttpGet]
        [ResponseType(typeof(BlogDto))]
        public IHttpActionResult ListBlogsForSubscribedUser(int id)
        {
            List<Blog> Blogs = db.Blogs.Where(b => b.SubscribedUsers.Any(a => a.SubscribedUserID == id)).ToList();
            List<BlogDto> BlogDtos = new List<BlogDto>();

            Blogs.ForEach(b => BlogDtos.Add(new BlogDto()
            {
                BlogID = b.BlogID,
                Title = b.Title,
                Content = b.Content
            }));
            return Ok(BlogDtos);
        }

        ///Objective: Create a method that returns the selected blog by entering a interger value of the selected blog id
        /// <summary>
        /// Return the selected blog
        /// </summary>
        /// <param name="id">BlogID</param>
        /// <return>selected blog</return>
        ///<example>GET: api/BlogData/FindBlog/{id}</example>
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

        /// Objective: Create a method that allow us to access the selected survey by entering a interger value of the selected blog id
        /// then Update the selected blog user with JSON form data of the Blog model 
        /// <summary>
        /// Update the selected blog from the database
        /// </summary>
        /// <param name="id">BlogID</param>
        /// <param name="Blog">Blog JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        ///<example>POST: api/BlogData/UpdateBlog/{id}</example>
        // POST: api/BlogData/UpdateBlog/5
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

        ///Objective: Create a method that allow us to add a new blog
        /// <summary>
        /// Add a new blog into the database
        /// </summary>
        /// <param name="Blog">Blog User JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>POST: api/BlogData/AddBlog</example>
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

        /// Objective: Create a method that allow us to delete the selected blog by entering a interger value of the selected Blog id
        /// <summary>
        /// Remove the selected Blog from the database
        /// </summary>
        /// <param name="id">BlogID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        ///<example>POST: api/BlogData/DeleteBlog/{id}</example>
        // DELETE: api/BlogData/DeleteBlog/5
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
