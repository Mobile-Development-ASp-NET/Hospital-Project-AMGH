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

        /// <summary>
        /// List of all the doctors along with their respective departments.
        /// </summary>
        /// <returns>List of Doctors.</returns>
        // GET: api/DoctorDetailsData/ListDoctorDetails
        
        [HttpGet]
        [ResponseType(typeof(DoctorDetailDto))]
        public IHttpActionResult ListDoctorDetails()
        {
            List<DoctorDetails> doctorDetails = db.DoctorDetails.ToList();
            List<DoctorDetailDto> doctorDetailDtos = new List<DoctorDetailDto>();
            doctorDetails.ForEach(element => doctorDetailDtos.Add(new DoctorDetailDto()
            {
                DrId = element.DrId,
                DrFname = element.DrFname,
                DrLname = element.DrLname,
                DrEmail = element.DrEmail,
                DrBio = element.DrBio,
                DrPosition = element.DrPosition,
                DrStudies = element.DrStudies ,
                DepartmentId = element.DepartmentId,
                DepartmentName =element.Department.DepartmentName

            }));
            return Ok(doctorDetailDtos);
        }
       

        /// <summary>
        /// Finds a Doctor with specific Id.
        /// </summary>
        /// <param name="id">Doctor Id</param>
        /// <returns>A specific doctor details</returns>
        // GET: api/DoctorDetailsData/FindDoctorDetail/1
        [ResponseType(typeof(DoctorDetails))]
        [HttpGet]
        public IHttpActionResult FindDoctorDetail(int id)
        {
            DoctorDetails doctorDetails = db.DoctorDetails.Find(id);
            DoctorDetailDto doctorDetailDto = new DoctorDetailDto()
            {
                DrId = doctorDetails.DrId,
                DrFname = doctorDetails.DrFname,
                DrLname = doctorDetails.DrLname,
                DrBio = doctorDetails.DrBio,
                DrEmail = doctorDetails.DrEmail,
                DrPosition = doctorDetails.DrPosition,
                DrStudies = doctorDetails.DrStudies,
                DepartmentId = doctorDetails.DepartmentId,
                DepartmentName = doctorDetails.Department.DepartmentName
            };
            if (doctorDetails == null)
            {
                return NotFound();
            }

            return Ok(doctorDetailDto);
        }
        /// <summary>
        /// Gathers informatin about all the doctors in a specific department
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns>List of Doctor Details</returns>
        /// GET: api/DoctorDetailsData/ListDoctorDetailForDepartment/1

        [HttpGet]
        [ResponseType(typeof(DoctorDetailDto))]
        public IHttpActionResult ListDoctorDetailForDepartment(int id)
        {
            List<DoctorDetails> doctorDetails = db.DoctorDetails.Where(a=>a.DepartmentId == id).ToList();
            List<DoctorDetailDto> doctorDetailDtos = new List<DoctorDetailDto>();
            doctorDetails.ForEach(element => doctorDetailDtos.Add(new DoctorDetailDto()
            {
                DrId = element.DrId,
                DrFname = element.DrFname,
                DrLname = element.DrLname,
                DrEmail = element.DrEmail,
                DrBio = element.DrBio,
                DrPosition = element.DrPosition,
                DrStudies = element.DrStudies,
                DepartmentId = element.DepartmentId,
                DepartmentName = element.Department.DepartmentName

            }));
            return Ok(doctorDetailDtos);

        }
        /// <summary>
        /// Updates the doctor information for specific Id
        /// </summary>
        /// <param name="id">Doctor Id</param>
        /// <param name="doctorDetails"></param>
        /// <returns>  
        ///      HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        // POST: api/DoctorDetailsData/UpdateDoctorDetail/3
        // curl -H "Content-Type:application/json" -d @DoctorDetail.json https://localhost:44342/api/DoctorDetailsData/UpdateDoctorDetail/3

        [HttpPost]
        [ResponseType(typeof(void))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateDoctorDetail(int id, DoctorDetails doctorDetails)
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

        /// <summary>
        /// Adds a new Doctor to the database.
        /// </summary>
        /// <param name="doctorDetails">Doctor detail Model</param>
        /// <returns>
        ///  HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        // curl -d @DoctorDetail.json -H "Content-type:application/json" https://localhost:44342/api/DoctorDetailsData/AddDoctor
        // POST: api/DoctorDetailsData/AddDoctor
        [ResponseType(typeof(DoctorDetails))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDoctor(DoctorDetails doctorDetails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DoctorDetails.Add(doctorDetails);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = doctorDetails.DrId }, doctorDetails);
        }
        /// <summary>
        /// Delete a spefic doctor
        /// </summary>
        /// <param name="id">Doctor Id</param>
        /// <returns>
        ///  HEADER: 200 (OK)
        ///     or
        ///     HEADER: 404 (NOT FOUND)
        /// </returns>
        //POST: api/DoctorDetailsData/DeletDoctorDetail/3
        // curl -d "" https://localhost:44324/api/doctorDetailsData/deleteDoctorDetail/3
        [HttpPost]
        [ResponseType(typeof(DoctorDetails))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteDoctorDetail(int id)
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