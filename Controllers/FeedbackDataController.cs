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

        /// <summary>
        /// Gathers information about all feedbacks related to a particular dr id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all feedbacks in the database, including their associated doctors matched with a particular dr ID
        /// </returns>
        /// <param name="id">DrId</param>
        /// <example>
        /// GET: api/FeedbackData/ListFeedbacksForDotor/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(FeedbackDto))]

        public IHttpActionResult ListFeedbacksForDoctor(int id)
        {

            List<Feedback> Feedbacks = db.Feedbacks.Where(a => a.DrId == id).ToList();
            List<FeedbackDto> FeedbackDtos = new List<FeedbackDto>();

            Feedbacks.ForEach(a => FeedbackDtos.Add(new FeedbackDto()
            {
                FeedbackId = a.FeedbackId,
                DrId = a.DoctorDetails.DrId,
                DrFname = a.DoctorDetails.DrFname,
                DrLname = a.DoctorDetails.DrLname,
                FeedbackContent = a.FeedbackContent,
                FeedbackDate = a.FeedbackDate
            }));

            return Ok(FeedbackDtos);
        }

        /// <summary>
        /// Returns a particular Feedback in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Feedback in the system matching up to the Feedback ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Feedback</param>
        /// <example>
        /// GET: api/FeedbackData/FindFeedback/3
        /// </example>
        [ResponseType(typeof(FeedbackDto))]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult FindFeedback(int id)
        {
            Feedback Feedback = db.Feedbacks.Find(id);
            FeedbackDto FeedbackDto = new FeedbackDto()
            {
                FeedbackId = Feedback.FeedbackId,
                UserId = Feedback.UserId,
                UserName = Feedback.ApplicationUser.UserName,
                DrId = Feedback.DoctorDetails.DrId,
                DrFname = Feedback.DoctorDetails.DrFname,
                DrLname = Feedback.DoctorDetails.DrLname,
                FeedbackContent = Feedback.FeedbackContent,
                FeedbackDate = Feedback.FeedbackDate
            };
            if (Feedback == null)
            {
                return NotFound();
            }

            return Ok(FeedbackDto);
        }

        /// <summary>
        /// Updates a particular Feedback in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Feedback ID primary key</param>
        /// <param name="Feedback">JSON FORM DATA of a Feedback</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/FeedbackData/UpdateFeedback/3
        /// FORM DATA: Feedback JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateFeedback(int id, Feedback Feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Feedback.FeedbackId)
            {
                return BadRequest();
            }

            db.Entry(Feedback).State = EntityState.Modified;

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

        /// <summary>
        /// Adds a Feedback to the system
        /// </summary>
        /// <param name="Feedback">JSON FORM DATA of a Feedback</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Feedback ID, Feedback Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/FeedbackData/AddFeedback
        /// FORM DATA: Feedback JSON Object
        /// </example>
        [ResponseType(typeof(Feedback))]
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public IHttpActionResult AddFeedback(Feedback Feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Feedbacks.Add(Feedback);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Feedback.FeedbackId }, Feedback);
        }

        /// <summary>
        /// Deletes a Feedback from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Feedback</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/FeedbackData/DeleteFeedback/3
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Feedback))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteFeedback(int id)
        {
            Feedback Feedback = db.Feedbacks.Find(id);
            if (Feedback == null)
            {
                return NotFound();
            }

            db.Feedbacks.Remove(Feedback);
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

        private bool FeedbackExists(int id)
        {
            return db.Feedbacks.Count(e => e.FeedbackId == id) > 0;
        }
    }
}