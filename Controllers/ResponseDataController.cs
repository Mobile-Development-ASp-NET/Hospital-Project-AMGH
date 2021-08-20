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
    public class ResponseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        ///Objective: Create a method that allow us to return all responses from the database
        /// <summary>
        /// return all responses from the database
        /// </summary>
        /// <returns>List of responses in the database</returns>
        /// <example>GET: api/ResponseData/ListResponses</example>
        [HttpGet]
        public List<ResponseDto> ListResponses()
        {
            List<Response> Responses = db.Responses.ToList();
            List<ResponseDto> ResponseDtos = new List<ResponseDto>();
            Responses.ForEach(r => ResponseDtos.Add(new ResponseDto()
            {
                QuestionID = r.Question.QuestionID,
                QuestionTitle =r.Question.QuestionTitle,
                ResponseID = r.ResponseID,
                Answer = r.Answer
            }));
            return ResponseDtos;
        }

        ///Objective: Create a method that allow us to return the selected response by entering a interger value of the selected response id
        /// <summary>
        /// Return the selected the response from the database
        /// </summary>
        /// <param name="id">responseID</param>
        /// <return>The selected response</return>
        ///<example>GET: api/ResponseData/FindQuestion/{id}</example>
        [ResponseType(typeof(Response))]
        public IHttpActionResult FindResponse(int id)
        {
            Response Response = db.Responses.Find(id);
            ResponseDto ResponseDto = new ResponseDto()
            {
                QuestionID = Response.Question.QuestionID,
                QuestionTitle = Response.Question.QuestionTitle,
                ResponseID = Response.ResponseID,
                Answer = Response.Answer

            };
            if (Response == null)
            {
                return NotFound();
            }

            return Ok(ResponseDto);
        }

        ///Objective: Create a method that allow us to access the selected response by entering a interger value of the selected response id
        ///Then Update the selected response with JSON form data of the response  model 
        /// <summary>
        /// Update the selected response  from the database
        /// </summary>
        /// <param name="id">response ID</param>
        /// <param name="response">response JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        ///<example>POST: api/ResponseData/UpdateQuestion/{id}</example>
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateResponse(int id, Response response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != response.ResponseID)
            {
                return BadRequest();
            }

            db.Entry(response).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponseExists(id))
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


        ///Objective: Create a method that allow us to add a new response by JSON form data of the response model into the database 
        /// <summary>
        /// Add a new response into the database
        /// </summary>
        /// <param name="response">response JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>POST: api/ResponseData/AddQuestion</example>
        [ResponseType(typeof(Response))]
        public IHttpActionResult AddResponse(Response response)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Responses.Add(response);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = response.ResponseID }, response);
        }

        ///Objective: Create a method that allow us to delete the selected response by entering a interger value of the selected response id
        /// <summary>
        /// Remove the selected the response from the database
        /// </summary>
        /// <param name="id">responseID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        ///<example>POST: api/ResponseData/DeleteQuestion/{id}</example>
        [ResponseType(typeof(Response))]
        public IHttpActionResult DeleteResponse(int id)
        {
            Response response = db.Responses.Find(id);
            if (response == null)
            {
                return NotFound();
            }

            db.Responses.Remove(response);
            db.SaveChanges();

            return Ok(response);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ResponseExists(int id)
        {
            return db.Responses.Count(e => e.ResponseID == id) > 0;
        }
    }
}