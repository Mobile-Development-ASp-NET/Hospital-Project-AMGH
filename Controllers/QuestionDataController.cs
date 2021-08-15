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
    public class QuestionDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        ///Objective: Create a method that allow us to return all questions from the database
        /// <summary>
        /// return all questions from the database
        /// </summary>
        /// <returns>List of questions in the database</returns>
        /// <example>GET: api/QuestionData/ListQuestions</example>
        [HttpGet]
        public List<QuestionDto> ListQuestions()
        {
            List<Question> Questions = db.Questions.ToList();
            List<QuestionDto> QuestionDtos = new List<QuestionDto>();

            Questions.ForEach(q => QuestionDtos.Add(new QuestionDto()
            {
                QuestionID = q.QuestionID,
                QuestionTitle = q.QuestionTitle,
                QuestionyDescription = q.QuestionyDescription
            }));

            return QuestionDtos;
        }

        ///Objective: Create a method that allow us to return all questions that are related to the selected survey
        ///by entering a interger value of the selected survey id
        /// <summary>
        /// Return all questions that are related to the survey from the database
        /// </summary>
        /// <param name="id">surveyID</param>
        /// <returns>List of questions that are related to the selected survey</returns>
        ///<example>GET: api/QuestionData/ListQuestionsForSurvey</example>
        [HttpGet]
        public List<QuestionDto> ListQuestionsForSurvey(int id)
        {
            List<Question> Questions = db.Questions.Where(q => q.Surveys.Any(
                s => s.SurveyID == id)).ToList();
            List<QuestionDto> QuestionDtos = new List<QuestionDto>();
            Questions.ForEach(q => QuestionDtos.Add(new QuestionDto()
            {
                QuestionID = q.QuestionID,
                QuestionTitle = q.QuestionTitle,
                QuestionyDescription = q.QuestionyDescription
            }));
            return QuestionDtos;
        }

        ///Objective: Create a method that allow us to return the selected question by entering a interger value of the selected question id
        /// <summary>
        /// Return the selected the question from the database
        /// </summary>
        /// <param name="id">questionID</param>
        /// <return>The selected question</return>
        ///<example>GET: api/QuestionData/FindQuestion/{id}</example>
        [HttpGet]
        [ResponseType(typeof(Question))]
        public IHttpActionResult FindQuestion(int id)
        {
            Question Question = db.Questions.Find(id);
            QuestionDto QuestionDto = new QuestionDto()
            {
                QuestionID = Question.QuestionID,
                QuestionyDescription = Question.QuestionyDescription,
                QuestionTitle = Question.QuestionTitle 
            };
            if (Question == null)
            {
                return NotFound();
            }

            return Ok(QuestionDto);
        }

        ///Objective: Create a method that allow us to access the selected question by entering a interger value of the selected question id
        ///Then Update the selected question with JSON form data of the question  model 
        /// <summary>
        /// Update the selected question  from the database
        /// </summary>
        /// <param name="id">question ID</param>
        /// <param name="question">question JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        ///<example>POST: api/QuestionData/UpdateQuestion/{id}</example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult UpdateQuestion(int id, Question questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != questions.QuestionID)
            {
                return BadRequest();
            }

            db.Entry(questions).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionsExists(id))
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

        ///Objective: Create a method that allow us to add a new question by JSON form data of the question model into the database 
        /// <summary>
        /// Add a new question into the database
        /// </summary>
        /// <param name="question">question JSON form data</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        ///<example>POST: api/QuestionData/AddQuestion</example>
        [ResponseType(typeof(Question))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddQuestion(Question questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(questions);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = questions.QuestionID }, questions);
        }

        ///Objective: Create a method that allow us to delete the selected question by entering a interger value of the selected question id
        /// <summary>
        /// Remove the selected the question from the database
        /// </summary>
        /// <param name="id">questionID</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        ///<example>POST: api/QuestionData/DeleteQuestion/{id}</example>
        [ResponseType(typeof(Question))]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult DeleteQuestion(int id)
        {
            Question questions = db.Questions.Find(id);
            if (questions == null)
            {
                return NotFound();
            }

            db.Questions.Remove(questions);
            db.SaveChanges();

            return Ok(questions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool QuestionsExists(int id)
        {
            return db.Questions.Count(e => e.QuestionID == id) > 0;
        }
    }
}