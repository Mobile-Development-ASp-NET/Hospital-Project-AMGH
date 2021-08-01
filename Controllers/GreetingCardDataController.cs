using System;
using System.IO;
using System.Web;
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
    public class GreetingCardDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        /// <summary>
        /// Returns all greeting cards in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all greeting card details in the database, including their associated patients.
        /// </returns>
        /// <example>
        /// GET: api/GreetingCardData/ListGreetingCards
        /// </example>
        [HttpGet]
        [ResponseType(typeof(GreetingCardDto))]
        public IHttpActionResult ListGreetingCards()
        {
            List<GreetingCard> GreetingCards = db.GreetingCards.ToList();
            List<GreetingCardDto> GreetingCardDtos = new List<GreetingCardDto>();

            GreetingCards.ForEach(c => GreetingCardDtos.Add(new GreetingCardDto()
            {
                CardId = c.CardId,
                SenderFirstName = c.SenderFirstName,
                SenderLastName = c.SenderLastName,
                CardType = c.CardType,
                CardMessage = c.CardMessage,
                CardHasPic = c.CardHasPic,
                PicExtension = c.PicExtension,
                AdmissionId = c.Admissions.AdmissionId,
                Room = c.Admissions.Room,
                Bed = c.Admissions.Bed
            }));

            return Ok(GreetingCardDtos);
        }

        /// <summary>
        /// Gathers information about all GreetingCards related to a particular admissions id
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: all GreetingCards in the database, including their associated admissions matched with a particular admissions ID
        /// </returns>
        /// <param name="id">Admission Id.</param>
        /// <example>
        /// GET: api/GreetingCardData/ListGreetingCardsForAdmission/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(GreetingCardDto))]
        public IHttpActionResult ListGreetingCardsForAdmission(int id)
        {
            List<GreetingCard> GreetingCards = db.GreetingCards.Where(c => c.AdmissionId == id).ToList();
            List<GreetingCardDto> GreetingCardDtos = new List<GreetingCardDto>();

            GreetingCards.ForEach(c => GreetingCardDtos.Add(new GreetingCardDto()
            {
                CardId = c.CardId,
                SenderFirstName = c.SenderFirstName,
                SenderLastName = c.SenderLastName,
                CardType = c.CardType,
                CardMessage = c.CardMessage,
                CardHasPic = c.CardHasPic,
                PicExtension = c.PicExtension,
                AdmissionId = c.Admissions.AdmissionId,
                Room = c.Admissions.Room,
                Bed = c.Admissions.Bed
            }));

            return Ok(GreetingCardDtos);
        }

        /// <summary>
        /// Returns GreetingCard of a particular id in the system.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A GreetingCard in the system matching up to the CardId primary key
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <param name="id">The primary key of the GreetingCard</param>
        /// <example>
        /// GET: api/GreetingCardData/FindGreetingCard/5
        /// </example>
        [ResponseType(typeof(GreetingCardDto))]
        [HttpGet]
        public IHttpActionResult FindGreetingCard(int id)
        {
            GreetingCard GreetingCard = db.GreetingCards.Find(id);
            GreetingCardDto GreetingCardDto = new GreetingCardDto()
            {
                CardId = GreetingCard.CardId,
                SenderFirstName = GreetingCard.SenderFirstName,
                SenderLastName = GreetingCard.SenderLastName,
                CardType = GreetingCard.CardType,
                CardMessage = GreetingCard.CardMessage,
                CardHasPic = GreetingCard.CardHasPic,
                PicExtension = GreetingCard.PicExtension,
                AdmissionId = GreetingCard.Admissions.AdmissionId,
                Room = GreetingCard.Admissions.Room,
                Bed = GreetingCard.Admissions.Bed
            };
            if (GreetingCard == null)
            {
                return NotFound();
            }

            return Ok(GreetingCardDto);
        }

        /// <summary>
        /// Updates a particular GreetingCard in the system with POST Data input
        /// </summary>
        /// <param name="id">Represents the Card Id primary key</param>
        /// <param name="GreetingCard">JSON FORM DATA of an GreetingCard</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/GreetingCardData/UpdateGreetingCard/5
        /// FORM DATA: GreetingCard JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult UpdateGreetingCard(int id, GreetingCard GreetingCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != GreetingCard.CardId)
            {

                return BadRequest();
            }

            db.Entry(GreetingCard).State = EntityState.Modified;
            // Picture update is handled by another method
            db.Entry(GreetingCard).Property(c => c.CardHasPic).IsModified = false;
            db.Entry(GreetingCard).Property(c => c.PicExtension).IsModified = false;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GreetingCardExists(id))
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
        /// Receives GreetingCard picture data, uploads it to the webserver and updates the GreetingCard's HasPic option
        /// </summary>
        /// <param name="id">the Card id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F GreetingCardpic=@file.jpg "https://localhost:xx/api/GreetingCarddata/uploadGreetingCardpic/2"
        /// POST: api/GreetingCardData/UpdateGreetingCardPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        [HttpPost]
        public IHttpActionResult UploadGreetingCardPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var GreetingCardPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (GreetingCardPic.ContentLength > 0)
                    {
                        //establish valid file types (can be changed to other file extensions if desired!)
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(GreetingCardPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/GreetingCards/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/GreetingCards/"), fn);

                                //save the file
                                GreetingCardPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;

                                //Update the GreetingCard haspic and picextension fields in the database
                                GreetingCard SelectedGreetingCard = db.GreetingCards.Find(id);
                                SelectedGreetingCard.CardHasPic = haspic;
                                SelectedGreetingCard.PicExtension = extension;
                                db.Entry(SelectedGreetingCard).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("GreetingCard Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                                return BadRequest();
                            }
                        }
                    }

                }

                return Ok();
            }
            else
            {
                //not multipart form data
                return BadRequest();

            }

        }

        /// <summary>
        /// Adds an GreetingCard to the system
        /// </summary>
        /// <param name="GreetingCard">JSON FORM DATA of an GreetingCard</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Card Id, GreetingCard Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/GreetingCardData/AddGreetingCard
        /// FORM DATA: GreetingCard JSON Object
        /// </example>
        [ResponseType(typeof(GreetingCard))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult AddGreetingCard(GreetingCard GreetingCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GreetingCards.Add(GreetingCard);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = GreetingCard.CardId }, GreetingCard);
        }

        /// <summary>
        /// Deletes an GreetingCard from the system by it's ID.
        /// </summary>
        /// <param name="id">The primary key of the GreetingCard</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/GreetingCardData/DeleteGreetingCard/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(GreetingCard))]
        [HttpPost]
        [Authorize]
        public IHttpActionResult DeleteGreetingCard(int id)
        {
            GreetingCard GreetingCard = db.GreetingCards.Find(id);
            if (GreetingCard == null)
            {
                return NotFound();
            }

            if (GreetingCard.CardHasPic && GreetingCard.PicExtension != "")
            {
                //also delete image from path
                string path = HttpContext.Current.Server.MapPath("~/Content/Images/GreetingCards/" + id + "." + GreetingCard.PicExtension);
                if (System.IO.File.Exists(path))
                {
                    Debug.WriteLine("File exists... preparing to delete!");
                    System.IO.File.Delete(path);
                }
            }

            db.GreetingCards.Remove(GreetingCard);
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

        private bool GreetingCardExists(int id)
        {
            return db.GreetingCards.Count(e => e.CardId == id) > 0;
        }
    }
}