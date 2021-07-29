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

        // GET: api/QuestionData
        [HttpGet]
        public List<QuestionDto> ListQuestions()
        {
            List<Questions> Questions = db.Questions.ToList();
            List<QuestionDto> QuestionDtos = new List<QuestionDto>();

            Questions.ForEach(q => QuestionDtos.Add(new QuestionDto()
            {
                QuestionID = q.QuestionID,
                QuestionTitle = q.QuestionTitle,
                QuestionyDescription = q.QuestionyDescription
            }));

            return QuestionDtos;
        }

        // GET: api/QuestionData/5
        [ResponseType(typeof(Questions))]
        public IHttpActionResult FindQuestion(int id)
        {
            Questions Question = db.Questions.Find(id);
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

        // PUT: api/QuestionData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateQuestions(int id, Questions questions)
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

        // POST: api/QuestionData
        [ResponseType(typeof(Questions))]
        [HttpPost]
        public IHttpActionResult AddQuestions(Questions questions)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Questions.Add(questions);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = questions.QuestionID }, questions);
        }

        // DELETE: api/QuestionData/5
        [ResponseType(typeof(Questions))]
        [HttpPost]
        public IHttpActionResult DeleteQuestions(int id)
        {
            Questions questions = db.Questions.Find(id);
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