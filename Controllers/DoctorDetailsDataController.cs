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
    public class DoctorDetailsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DoctorDetailsData
        public IQueryable<DoctorDetails> GetDoctorDetails()
        {
            return db.DoctorDetails;
        }

        // GET: api/DoctorDetailsData/5
        [ResponseType(typeof(DoctorDetails))]
        public IHttpActionResult GetDoctorDetails(int id)
        {
            DoctorDetails doctorDetails = db.DoctorDetails.Find(id);
            if (doctorDetails == null)
            {
                return NotFound();
            }

            return Ok(doctorDetails);
        }

        // PUT: api/DoctorDetailsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDoctorDetails(int id, DoctorDetails doctorDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != doctorDetails.DrId)
            {
                return BadRequest();
            }

            db.Entry(doctorDetails).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorDetailsExists(id))
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

        // POST: api/DoctorDetailsData
        [ResponseType(typeof(DoctorDetails))]
        public IHttpActionResult PostDoctorDetails(DoctorDetails doctorDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DoctorDetails.Add(doctorDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctorDetails.DrId }, doctorDetails);
        }

        // DELETE: api/DoctorDetailsData/5
        [ResponseType(typeof(DoctorDetails))]
        public IHttpActionResult DeleteDoctorDetails(int id)
        {
            DoctorDetails doctorDetails = db.DoctorDetails.Find(id);
            if (doctorDetails == null)
            {
                return NotFound();
            }

            db.DoctorDetails.Remove(doctorDetails);
            db.SaveChanges();

            return Ok(doctorDetails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DoctorDetailsExists(int id)
        {
            return db.DoctorDetails.Count(e => e.DrId == id) > 0;
        }
    }
}