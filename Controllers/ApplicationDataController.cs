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
    public class ApplicationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/ApplicationData/ListApplications
        // Admin Credential
        /// <summary>
        /// Admin will view the list of applications, and see what status they have.
        /// </summary>
        /// <returns>
        /// HEADER: 200 OK
        /// Content: all applications that were submitted 
        /// </returns>
        /// <example>
        /// GET: api/ApplicationData/ListApplications
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ApplicationDto))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ListApplications()
        {
            List<Application> Applications = db.Applications.ToList();
            List<ApplicationDto> ApplicationDto = new List<ApplicationDto>();

            Applications.ForEach(a => ApplicationDto.Add(new ApplicationDto()
            {
                ApplicationID = a.ApplicationID,
                ApplicationName = a.ApplicationName,
                ApplicationDOB = a.ApplicationDOB,
                ApplicationEmail = a.ApplicationEmail,
                ApplicationCriminalRecord = a.ApplicationCriminalRecord,
                ApplicationStatus = a.ApplicationStatus,
                PositionID = a.Position.PositionID,
                PositionJob = a.Position.PositionJob

            }));

            return Ok(ApplicationDto);
        }


        /// <summary>
        /// List Applications that are related to a specific position id
        /// </summary>
        /// <param name="id">Positions id</param>
        /// <returns>
        /// HEADER 200 OK
        /// CONTENT: all applications in the database, including the positions that are matched with a particular position id
        /// </returns>
        /// <example>
        /// GET: api/ApplicationData/ListApplicationsForPositions/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(ApplicationDto))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ListApplicationsForPosition(int id)
        {
            List<Application> Applications = db.Applications.Where(a => a.PositionID == id).ToList();
            List<ApplicationDto> ApplicationDtos = new List<ApplicationDto>();

            Applications.ForEach(a => ApplicationDtos.Add(new ApplicationDto()
            {
                ApplicationID = a.ApplicationID,
                ApplicationName = a.ApplicationName,
                ApplicationDOB = a.ApplicationDOB,
                ApplicationEmail = a.ApplicationEmail,
                ApplicationCriminalRecord = a.ApplicationCriminalRecord,
                ApplicationStatus = a.ApplicationStatus,
                PositionID = a.Position.PositionID,
                PositionJob = a.Position.PositionJob

            }));

            return Ok(ApplicationDtos);
        }

        // GET: api/ApplicationData/FindApplication/5
        // Admin Credential
        /// <summary>
        /// Finds an application base on ID
        /// </summary>
        /// <param name="id">primary key of the application</param>
        /// <returns>
        /// HEADER: 200 OK
        /// OR
        /// HEADER 404 NOT FOUND
        /// </returns>
        /// <example>
        /// GET: api/ApplicationData/FindApplication/5
        /// </example>
        [ResponseType(typeof(ApplicationDto))]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult FindApplication(int id)
        {
            Application Application = db.Applications.Find(id);
            ApplicationDto ApplicationDto = new ApplicationDto()
            {
                ApplicationID = Application.ApplicationID,
                ApplicationName = Application.ApplicationName,
                ApplicationDOB = Application.ApplicationDOB,
                ApplicationEmail = Application.ApplicationEmail,
                ApplicationCriminalRecord = Application.ApplicationCriminalRecord,
                ApplicationStatus = Application.ApplicationStatus,
                PositionID = Application.Position.PositionID,
                PositionJob = Application.Position.PositionJob
            };
            if (Application == null)
            {
                return NotFound();
            }

            return Ok(ApplicationDto);
        }

        // POST: api/ApplicationData/UpdateApplication/5
        // Admin Credential
        /// <summary>
        /// Admin updates the status of the application
        /// </summary>
        /// <param name="id">Application Id</param>
        /// <param name="application">JSON Form DATA of an application</param>
        /// <returns>
        /// HEADER 204 : Success No content request
        /// or
        /// HEADER 400 : Bad Request
        /// or
        /// HEADER 404 : Not Found
        /// </returns>
        /// <example>
        /// POST: api/ApplicationData/UpdateApplication/5
        /// FORM DATA: Application JSON Data
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateApplication(int id, Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != application.ApplicationID)
            {
                return BadRequest();
            }

            db.Entry(application).State = EntityState.Modified;

            try
            {
                db.SaveChanges(); // Error when updating the application
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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
        /// Adds an application to the system
        /// </summary>
        /// <param name="application">JSON FORM DATA of an application</param>
        /// <returns>
        /// HEADER 201 : Created
        /// CONTENT : Application ID, Application Data
        /// or
        /// HEADER 400 : Bad Request
        /// </returns>
        /// <example>
        /// POST: api/ApplicationData/AddApplication
        /// FORM DATA: Application JSON Object
        /// </example>
        // POST: api/ApplicationData/AddApplication
        // Since we want to even allow un login people to make an application
        [ResponseType(typeof(Application))]
        [HttpPost]
        public IHttpActionResult AddApplication(Application application)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Applications.Add(application);
            db.SaveChanges(); 

            return CreatedAtRoute("DefaultApi", new { id = application.ApplicationID }, application);
        }

        /// <summary>
        /// Deletes an Application from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the application</param>
        /// <returns>
        /// HEADER 200 : Ok 
        /// or
        /// HEADER 404 : Not Found
        /// </returns>
        /// <example>
        /// POST: api/ApplicationData/DeleteApplication/1
        /// FORM DATA: Empty
        /// </example>
        // DELETE: api/ApplicationData/DeleteApplication/5
        // Admin Credential
        [ResponseType(typeof(Application))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteApplication(int id)
        {
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return NotFound();
            }

            db.Applications.Remove(application);
            db.SaveChanges();

            return Ok(application);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationExists(int id)
        {
            return db.Applications.Count(e => e.ApplicationID == id) > 0;
        }
    }
}