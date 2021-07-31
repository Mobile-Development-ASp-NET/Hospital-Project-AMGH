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
    public class AdmissionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Admissions in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all Admissions in the database, including their associated doctors and username.
        /// </returns>
        /// <example>
        /// GET: api/AdmissionData/ListAdmissions
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AdmissionDto))]
        public IHttpActionResult ListAdmissions()
        {
            List<Admission> Admissions = db.Admissions.ToList();
            List<AdmissionDto> AdmissionDtos = new List<AdmissionDto>();

            Admissions.ForEach(a => AdmissionDtos.Add(new AdmissionDto()
            {
                AdmissionId = a.AdmissionId,
                Room = a.Room,
                Bed = a.Bed,
                DrId = a.DoctorDetails.DrId,
                DrFname = a.DoctorDetails.DrFname,
                DrLname  =a.DoctorDetails.DrLname,
                UserId = a.UserId,
                UserName  =a.ApplicationUser.UserName
            }));

            return Ok(AdmissionDtos);
        }

        /// <summary>
        /// Returns a particular admission  in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An Admission in the system matching up to the Admission ID primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the Admissions</param>
        /// <example>
        /// GET: api/AdmissionData/FindAdmission/5
        /// </example>
        [ResponseType(typeof(AdmissionDto))]
        [HttpGet]
        public IHttpActionResult FindAdmission(int id)
        {
            Admission Admission = db.Admissions.Find(id);
            AdmissionDto AdmissionDto = new AdmissionDto()
            {
                AdmissionId = Admission.AdmissionId,
                Room = Admission.Room,
                Bed = Admission.Bed,
                DrId = Admission.DoctorDetails.DrId,
                DrFname = Admission.DoctorDetails.DrFname,
                DrLname = Admission.DoctorDetails.DrLname,
                UserId = Admission.UserId,
                UserName = Admission.ApplicationUser.UserName
            };
            if (Admission == null)
            {
                return NotFound();
            }

            return Ok(AdmissionDto);
        }

        /// <summary>
        /// Updates a particular Admission in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Admission ID primary key</param>
        /// <param name="Admission">JSON FORM DATA of an Admissions</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/AdmissionData/UpdateAdmission/5
        /// FORM DATA: Admission JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAdmissions(int id, Admission Admission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Admission.AdmissionId)
            {

                return BadRequest();
            }

            db.Entry(Admission).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmissionExists(id))
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
        /// Adds an Admission to the system
        /// </summary>
        /// <param name="Admission">JSON FORM DATA of an Admission</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Admission ID, Admission Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/AdmissionData/AddAdmission
        /// FORM DATA: Admission JSON Object
        /// </example>
        [ResponseType(typeof(Admission))]
        [HttpPost]
        public IHttpActionResult AddAdmissions(Admission Admission)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Admissions.Add(Admission);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Admission.AdmissionId }, Admission);
        }

        /// <summary>
        /// Deletes an Admission from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the Admission</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/AdmissionData/DeleteAdmission/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Admission))]
        [HttpPost]
        public IHttpActionResult DeleteAdmission(int id)
        {
            Admission Admission = db.Admissions.Find(id);
            if (Admission == null)
            {
                return NotFound();
            }

            db.Admissions.Remove(Admission);
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
        private bool AdmissionExists(int id)
        {
            return db.Admissions.Count(e => e.AdmissionId == id) > 0;
        }
    }
}
