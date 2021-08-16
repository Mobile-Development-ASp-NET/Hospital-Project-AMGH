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
    public class SurveyDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ///Objective: Create a method that allow us to return all surveys from the database
        /// <summary>
        /// return all surveys from the database
        /// </summary>
        /// <returns>List of surveys in the database</returns>
        /// <example>GET: api/surveyData/ListSurveys</example>
        [HttpGet]
        public List<SurveyDto> ListSurveys()
        {
            List<Survey> Surveys = db.Surveys.ToList();
            List<SurveyDto> SurveyDtos = new List<SurveyDto>();

            Surveys.ForEach(s => SurveyDtos.Add(new SurveyDto()
            {
                SurveyID = s.SurveyID,
                SurveyDescription = s.SurveyDescription,
                SurveyTitle = s.SurveyTitle
            }));

            return SurveyDtos;
        }



        ///Objective: Create a method that allow us to return the selected survey by entering a interger value of the selected survey id
        /// <summary>
        /// Return the selected the survey from the database
        /// </summary>
        /// <param name="id">surveyID</param>
        /// <return>The selected survey</return>
        ///<example>GET: api/surveyData/Findsurvey/{id}</example>
        [ResponseType(typeof(Survey))]
        [HttpGet]
        public IHttpActionResult FindSurvey(int id)
        {
            Survey Survey = db.Surveys.Find(id);
            SurveyDto SurveyDto = new SurveyDto()
            {
                SurveyID = Survey.SurveyID,
                SurveyDescription = Survey.SurveyDescription,
                SurveyTitle = Survey.SurveyTitle
            };
            if (Survey == null)
            {
                return NotFound();
            }

            return Ok(SurveyDto);
        }


        ///Objective: Create a method that allow us to return all surveys that are related to the selected question
        ///by entering a interger value of the selected question id
        /// <summary>
        /// Return all surveys that are related to the selected question from the database
        /// </summary>
        /// <param name="id">questionID</param>
        /// <returns>List of surveys that are related to the selected question</returns>
        ///<example>GET: api/SurveyData/ListSurveysForQuestion</example>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<SurveyDto> ListSurveysForQuestion(int id)
        {
            List<Survey> Surveys = db.Surveys.Where(s => s.Questions.Any(
                q => q.QuestionID == id)).ToList();
            List<SurveyDto> SurveyDtos = new List<SurveyDto>();
            Surveys.ForEach(s => SurveyDtos.Add(new SurveyDto()
            {
                SurveyID = s.SurveyID,
                SurveyTitle = s.SurveyTitle,
                SurveyDescription = s.SurveyDescription
            }));
            return SurveyDtos;
        }


        ///Objective: Create a method that allow us to access the selected survey by entering a interger value of the selected survey id
        ///Then Update the selected survey with JSON form data of the survey  model 
        /// <summary>
        /// Update the selected survey from the database
        /// </summary>
        /// <param name="id">survey ID</param>
        /// <param name="survey ">survey  JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        ///<example>POST: api/surveyData/UpdateSurveys/{id}</example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateSurvey(int id, Survey surveys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != surveys.SurveyID)
            {
                return BadRequest();
            }

            db.Entry(surveys).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveysExists(id))
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



        ///Objective: Create a method that allow us to add a new survey by JSON form data of the actor model into the database 
        /// <summary>
        /// Add a new survey into the database
        /// </summary>
        /// <param name="survey">survey JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>POST: api/SurveyrData/AddSurveys</example>
        [ResponseType(typeof(Survey))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddSurvey(Survey surveys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Surveys.Add(surveys);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = surveys.SurveyID }, surveys);
        }


        ///Objective: Create a method that allow us to delete the selected Survey by entering a interger value of the selected Survey id
        /// <summary>
        /// Remove the selected the Survey from the database
        /// </summary>
        /// <param name="id">SurveyID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        ///<example>POST: api/SurveyData/DeleteSurveys/{id}</example>
        [ResponseType(typeof(Survey))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteSurvey(int id)
        {
            Survey surveys = db.Surveys.Find(id);
            if (surveys == null)
            {
                return NotFound();
            }

            db.Surveys.Remove(surveys);
            db.SaveChanges();

            return Ok(surveys);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SurveysExists(int id)
        {
            return db.Surveys.Count(e => e.SurveyID == id) > 0;
        }
    }
}