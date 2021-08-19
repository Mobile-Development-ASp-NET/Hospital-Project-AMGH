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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// To list all the departments.
        /// </summary>
        /// <returns>A list of all departments.</returns>
        // GET: api/DepartmentData/ListDepartments
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult ListDepartments()
        {
            List<Department> departments = db.Departments.ToList();
            List<DepartmentDto> departmentDtos = new List<DepartmentDto>();
            Debug.WriteLine(departments);
            departments.ForEach(element => departmentDtos.Add(new DepartmentDto()
            {
                DepartmentID = element.DepartmentID,
                DepartmentName = element.DepartmentName,
                DepartmentDescription = element.DepartmentDescription

            })); 
            return Ok(departmentDtos);
        }

        /// <summary>
        /// Finds a particular department detail with the help of id.
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns>A sepcific Department.</returns>
        // GET: api/DepartmentData/FindDepartment/1
        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            DepartmentDto departmentdto = new DepartmentDto()
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName,
                DepartmentDescription = department.DepartmentDescription
            };
            if (department == null)
            {
                return NotFound();
            }

            return Ok(departmentdto);
        }

        // POST: api/DepartmentData/UpdateDepartment/3
        // curl -H "Content-Type:application/json" -d @department.json https://localhost:44342/api/departmentdata/updatedepartment/3
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateDepartment(int id, Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentID)
            {
                return BadRequest();
            }

            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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

        // POST: api/DepartmentData/AddDepartment
        // curl -d @department.json -H "Content-type:application/json" http://localhost:44342/api/DepartmentData/AddDepartment
        // POST: api/DepartmentData
        [HttpPost]
        [ResponseType(typeof(Department))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddDepartment(Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Departments.Add(department);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = department.DepartmentID }, department);
        }

        // DELETE: api/DepartmentData/DeleteDepartment/3
        // curl -d "" https://localhost:44324/api/departmentdata/deletedepartment/3
        [HttpPost]
        [ResponseType(typeof(Department))]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok(department);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }
    }
}