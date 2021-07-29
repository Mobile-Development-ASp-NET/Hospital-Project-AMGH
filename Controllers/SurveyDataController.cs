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

        // GET: api/SurveyData
        [HttpGet]
        public List<SurveyDto> ListSurveys()
        {
            List<Surveys> Surveys = db.Surveys.ToList();
            List<SurveyDto> SurveyDtos = new List<SurveyDto>();

            Surveys.ForEach(s => SurveyDtos.Add(new SurveyDto()
            {
                SurveyID = s.SurveyID,
                SurveyDescription = s.SurveyDescription,
                SurveyTitle = s.SurveyTitle
            }));

            return SurveyDtos;
        }

        // GET: api/SurveyData/5
        [ResponseType(typeof(Surveys))]
        public IHttpActionResult FindSurvey(int id)
        {
            Surveys Survey = db.Surveys.Find(id);
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

        // PUT: api/SurveyData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateSurveys(int id, Surveys surveys)
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

        // POST: api/SurveyData
        [ResponseType(typeof(Surveys))]
        [HttpPost]
        public IHttpActionResult AddSurveys(Surveys surveys)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Surveys.Add(surveys);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = surveys.SurveyID }, surveys);
        }

        // DELETE: api/SurveyData/5
        [ResponseType(typeof(Surveys))]
        [HttpPost]
        public IHttpActionResult DeleteSurveys(int id)
        {
            Surveys surveys = db.Surveys.Find(id);
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