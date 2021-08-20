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
    public class SubscribedUserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// Objective: Create a method that returns all subscribed users from the database
        /// <summary>
        /// return all subscribed users from the database
        /// </summary>
        /// <returns>List of subscribed users </returns>
        /// <example>GET: api/SubscribedUserData/ListSubscribedUsers</example>
        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IEnumerable<SubscribedUserDto> ListSubscribedUsers()
        {
            List<SubscribedUser> subscribedUsers = db.SubscribedUsers.ToList();
            List<SubscribedUserDto> SubscribedUserDtos = new List<SubscribedUserDto>();
            subscribedUsers.ForEach(e => SubscribedUserDtos.Add(new SubscribedUserDto()
            {
                SubscribedUserID = e.SubscribedUserID,
                Name = e.Name,
                Email = e.Email
            }));
            return SubscribedUserDtos;
        }

        ///Objective: Create a method that allow us to return all subscribed users that are related to the blog
        ///by entering a interger value of the selected blog id
        /// <summary>
        /// Return all subscribed users that are related to the selected blog from the database
        /// </summary>
        /// <param name="id">blogID</param>
        ///<example>GET: api/SubscribedUserData/ListSubscribedUsersForBlog</example>
        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IHttpActionResult ListSubscribedUsersForBlog(int id)
        {
            List<SubscribedUser> SubscribedUsers = db.SubscribedUsers.Where(b => b.Blogs.Any(a => a.BlogID == id)).ToList();
            List<SubscribedUserDto> SubscribedUserDtos = new List<SubscribedUserDto>();

            SubscribedUsers.ForEach(b => SubscribedUserDtos.Add(new SubscribedUserDto()
            {
                SubscribedUserID = b.SubscribedUserID,
                Name = b.Name,
                Email = b.Email
            }));
            return Ok(SubscribedUserDtos);
        }

        ///Objective: Create a method that returns the selected user by entering a interger value of the selected user id
        /// <summary>
        /// Return the selected subscribed user
        /// </summary>
        /// <param name="id">SubscribedUserID</param>
        /// <return>selected subscribed user</return>
        ///<example>GET: api/SubscribedUserData/FindSubscribedUser/{id}</example>
        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IHttpActionResult FindSubscribedUser(int id)
        {
            SubscribedUser subscribedUser = db.SubscribedUsers.Find(id);
            SubscribedUserDto subscribedUserDto = new SubscribedUserDto()
            {
                SubscribedUserID = subscribedUser.SubscribedUserID,
                Name = subscribedUser.Name,
                Email = subscribedUser.Email
            };
            if (subscribedUser == null)
            {
                return NotFound();
            }

            return Ok(subscribedUserDto);
        }

        /// Objective: Create a method that allow us to access the selected survey by entering a interger value of the selected user id
        /// then Update the selected subscribed user with JSON form data of the subscribedUser model 
        /// <summary>
        /// Update the selected subscribed user from the database
        /// </summary>
        /// <param name="id">SubscribedUserID</param>
        /// <param name="SubscribedUser">SubscribedUser JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        ///<example>POST: api/SubscribedUserData/UpdateSubscribedUser/{id}</example>

        [HttpPost]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateSubscribedUser(int id, SubscribedUser subscribedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subscribedUser.SubscribedUserID)
            {
                return BadRequest();
            }

            db.Entry(subscribedUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubscribedUserExists(id))
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

        ///Objective: Create a method that allow us to add a new SubscribedUser
        /// <summary>
        /// Add a new SubscribedUser into the database
        /// </summary>
        /// <param name="SubscribedUser">Subscribed User JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>POST: api/SubscribedUserData/AddSubscribedUser</example>
        [HttpPost]
        [ResponseType(typeof(SubscribedUser))]
        public IHttpActionResult AddSubscribedUser(SubscribedUser subscribedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SubscribedUsers.Add(subscribedUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subscribedUser.SubscribedUserID }, subscribedUser);
        }

        /// Objective: Create a method that allow us to delete the selected SubscribedUser by entering a interger value of the selected SubscribedUser id
        /// <summary>
        /// Remove the selected SubscribedUser from the database
        /// </summary>
        /// <param name="id">SubscribedUserID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        ///<example>POST: api/SubscribedUserData/DeleteSubscribedUser/{id}</example>
        [HttpPost]
        [ResponseType(typeof(SubscribedUser))]
        public IHttpActionResult DeleteSubscribedUser(int id)
        {
            SubscribedUser subscribedUser = db.SubscribedUsers.Find(id);
            if (subscribedUser == null)
            {
                return NotFound();
            }

            db.SubscribedUsers.Remove(subscribedUser);
            db.SaveChanges();

            return Ok(subscribedUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubscribedUserExists(int id)
        {
            return db.SubscribedUsers.Count(e => e.SubscribedUserID == id) > 0;
        }
    }
}
