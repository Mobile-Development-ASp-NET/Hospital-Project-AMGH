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
using System.Diagnostics;

namespace Hospital_Project.Controllers
{
    public class FeedbackDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Feedbacks in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Feedbacks in the database, including their associated doctors and username.
        /// </returns>
        /// <example>
        /// GET: api/FeedbackData/ListFeedbacks
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FeedbackDto))]
        public IHttpActionResult ListFeedbacks()
        {
            List<Feedback> Feedbacks = db.Feedbacks.ToList();
            List<FeedbackDto> FeedbackDtos = new List<FeedbackDto>();

            Feedbacks.ForEach(a => FeedbackDtos.Add(new FeedbackDto()
            {
                FeedbackId = a.FeedbackId,
                UserId = a.UserId,
                UserName = a.ApplicationUser.UserName,
                DrId = a.DoctorDetails.DrId,
                DrFname = a.DoctorDetails.DrFname,
                DrLname = a.DoctorDetails.DrLname,
                FeedbackContent = a.FeedbackContent,
                FeedbackDate = a.FeedbackDate                
                
            }));

            return Ok(FeedbackDtos);
        }

        // GET: api/FeedbackData/5
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult GetFeedback(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            return Ok(feedback);
        }

        // PUT: api/FeedbackData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFeedback(int id, Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedback.FeedbackId)
            {
                return BadRequest();
            }

            db.Entry(feedback).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/FeedbackData
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult PostFeedback(Feedback feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = feedback.FeedbackId }, feedback);
        }

        // DELETE: api/FeedbackData/5
        [ResponseType(typeof(Feedback))]
        public IHttpActionResult DeleteFeedback(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return NotFound();
            }

            db.Feedbacks.Remove(feedback);
            db.SaveChanges();

            return Ok(feedback);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FeedbackExists(int id)
        {
            return db.Feedbacks.Count(e => e.FeedbackId == id) > 0;
        }
    }
}