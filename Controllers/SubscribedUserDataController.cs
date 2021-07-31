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
using Hospital_Project.Models;

namespace Hospital_Project.Controllers
{
    public class SubscribedUserDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SubscribedUserData/ListSubscribedUsers
        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IEnumerable<SubscribedUserDto> ListSubscribedUsers()
        {
            List<SubscribedUser> SubscribedUsers = db.SubscribedUses.ToList();
            List<SubscribedUserDto> SubscribedUserDtos = new List<SubscribedUserDto>();
            SubscribedUsers.ForEach(e => SubscribedUserDtos.Add(new SubscribedUserDto()
            {
                SubscribedUserID = e.SubscribedUserID,
                Name = e.Name,
                Email = e.Email
            }));
            return SubscribedUserDtos;
        }

        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IHttpActionResult ListSubscribedUsersForBlog(int id)
        {
            List<SubscribedUser> SubscribedUsers = db.SubscribedUses.Where(b => b.Blogs.Any(a => a.BlogID == id)).ToList();
            List<SubscribedUserDto> SubscribedUserDtos = new List<SubscribedUserDto>();

            SubscribedUsers.ForEach(b => SubscribedUserDtos.Add(new SubscribedUserDto()
            {
                SubscribedUserID = b.SubscribedUserID,
                Name = b.Name,
                Email = b.Email
            }));
            return Ok(SubscribedUserDtos);
        }

        [HttpGet]
        [ResponseType(typeof(SubscribedUserDto))]
        public IHttpActionResult FindSubscribedUser(int id)
        {
            SubscribedUser subscribedUser = db.SubscribedUses.Find(id);
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

        // PUT: api/SubscribedUserData/UpdateSubscribedUser/5
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

        [HttpPost]
        [ResponseType(typeof(SubscribedUser))]
        public IHttpActionResult AddSubscribedUser(SubscribedUser subscribedUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SubscribedUses.Add(subscribedUser);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = subscribedUser.SubscribedUserID }, subscribedUser);
        }

        [HttpPost]
        [ResponseType(typeof(SubscribedUser))]
        public IHttpActionResult DeleteSubscribedUser(int id)
        {
            SubscribedUser subscribedUser = db.SubscribedUses.Find(id);
            if (subscribedUser == null)
            {
                return NotFound();
            }

            db.SubscribedUses.Remove(subscribedUser);
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
            return db.SubscribedUses.Count(e => e.SubscribedUserID == id) > 0;
        }
    }
}