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
    public class PositionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // Once the Department Table is made you need to make a ListPositionsforDepartments


        // Admin Credentials
        /// <summary>
        /// List all positions that are available follow by dates
        /// </summary>
        /// <returns>
        /// HEADER 200 : OK
        /// CONTENT: All information regarding the position table
        /// </returns>
        /// <example>
        // GET: api/PositionData/ListPositions
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PositionDto))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ListPositions()
        {
            List<Position> Positions = db.Positions.ToList();
            List<PositionDto> PositionDtos = new List<PositionDto>();

            Positions.ForEach(p => PositionDtos.Add(new PositionDto()
            {
                PositionID = p.PositionID,
                PositionJob = p.PositionJob,
                PositionDescription = p.PositionDescription,
                PositionPostedDate = p.PositionPostedDate,
                ApplicationDeadLine = p.ApplicationDeadLine,
                DepartmentID = p.Department.DepartmentID,
                DepartmentName = p.Department.DepartmentName
            }));

            return Ok(PositionDtos);
        }

        /// <summary>
        /// Gathers information that all positions related to a particular department ID
        /// </summary>
        /// <param name="id">Department ID</param>
        /// <returns>
        /// HEADER 200 : Ok
        /// CONTENT : all positions in the database, including their associated department that matched with a picticular Department ID
        /// </returns>
        /// <example>
        //  GET: api/PositionData/ListPositionForDepartment/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PositionDto))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult ListPostionsForDepartment(int id)
        {
            List<Position> Positions = db.Positions.Where(p => p.DepartmentID == id).ToList();
            List<PositionDto> PositionsDtos = new List<PositionDto>();

            Positions.ForEach(p => PositionsDtos.Add(new PositionDto()
            { 
                PositionID = p.PositionID,
                PositionJob = p.PositionJob,
                PositionDescription = p.PositionDescription,
                PositionPostedDate = p.PositionPostedDate,
                ApplicationDeadLine = p.ApplicationDeadLine,
                DepartmentID = p.Department.DepartmentID,
                DepartmentName = p.Department.DepartmentName
            }));

            return Ok(PositionsDtos);
        }

        // Admin Credentials
        /// <summary>
        /// Find the info on a position base on ID
        /// </summary>
        /// <param name="id">Position Primary Key</param>
        /// <returns>
        /// HEADER 200 : OK
        /// CONTENT: All information of the position base on Position Primary Key
        /// or
        /// HEADER 404 : Not Found
        /// </returns>
        /// <example>
        // GET: api/PositionData/FindPosition/5
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PositionDto))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult FindPosition(int id)
        {
            Position Positions = db.Positions.Find(id);
            PositionDto PositionDtos = new PositionDto()
            {
                PositionID = Positions.PositionID,
                PositionJob = Positions.PositionJob,
                PositionDescription = Positions.PositionDescription,
                PositionPostedDate = Positions.PositionPostedDate,
                ApplicationDeadLine = Positions.ApplicationDeadLine,
                DepartmentID = Positions.Department.DepartmentID,
                DepartmentName = Positions.Department.DepartmentName
            };
            if (Positions == null)
            {
                return NotFound();
            }
            return Ok(PositionDtos);
        }

        // Admin Credentials
        /// <summary>
        /// Updaes a picticular Position in the system with a POST Data input
        /// </summary>
        /// <param name="id">Position Primary Key</param>
        /// <param name="Position">JSON FORM DATA of a Position</param>
        /// <returns>
        /// HEADER 204 : Success, No content response
        /// or
        /// HEADER 400 : Bad Request
        /// or
        /// HEADER 404 : Not Found
        /// </returns>
        /// <example>
        // PUT: api/PositionData/UpdatePosition/5
        // FORM DATA: Position JSON Object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdatePosition(int id, Position Position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Position.PositionID)
            {
                return BadRequest();
            }

            db.Entry(Position).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PositionExists(id))
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

        // Admin Credentials
        /// <summary>
        /// Adds a Position to the system
        /// </summary>
        /// <param name="Position">JSON FORM DATA of a Position</param>
        /// <returns>
        /// HEADER 201 : Created
        /// or
        /// HEADER 400 : Bad Request
        /// </returns>
        /// <example>
        // POST: api/PositionData/AddPosition
        // FORM DATA: Position JSON Object 
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Position))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddPosition(Position Position)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Positions.Add(Position);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = Position.PositionID }, Position);
        }

        // Admin Credentials
        /// <summary>
        /// Deletes a Position from the system base on it's ID.
        /// </summary>
        /// <param name="id">Primary Key of the Position</param>
        /// <returns>
        /// HEADER 200 : Ok
        /// or
        /// HEADER 404 : Not Found
        /// </returns>
        /// <example>
        // DELETE: api/PositionData/DeletePosition/5
        // FORM DATA: empty
        /// </example>
        [ResponseType(typeof(Position))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeletePosition(int id)
        {
            Position Position = db.Positions.Find(id);
            if (Position == null)
            {
                return NotFound();
            }

            db.Positions.Remove(Position);
            db.SaveChanges();

            return Ok(Position);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PositionExists(int id)
        {
            return db.Positions.Count(e => e.PositionID == id) > 0;
        }
    }
}