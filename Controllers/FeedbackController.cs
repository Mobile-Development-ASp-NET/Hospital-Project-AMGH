using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System.Web.Script.Serialization;


namespace Hospital_Project.Controllers
{
    public class FeedbackController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static FeedbackController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }

        // GET: Feedback/List

        public ActionResult List()
        {
            //objective: communicate with our Feedback data api to retrieve a list of Feedbacks
            //curl https://localhost:44342/api/Feedbackdata/listFeedbacks


            string url = "Feedbackdata/listFeedbacks";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<FeedbackDto> Feedbacks = response.Content.ReadAsAsync<IEnumerable<FeedbackDto>>().Result;
            //Debug.WriteLine("Number of Feedbacks received : ");
            //Debug.WriteLine(Feedbacks.Count());


            return View(Feedbacks);
        }

        // GET: Feedback/Details/3
        public ActionResult Details(int id)
        {
            //objective: communicate with our Feedback data api to retrieve one Feedback
            //curl https://localhost:44342/api/Feedbackdata/findFeedback/{id}

            DetailsFeedback ViewModel = new DetailsFeedback();

            string url = "Feedbackdata/findFeedback/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            FeedbackDto SelectedFeedback = response.Content.ReadAsAsync<FeedbackDto>().Result;
            Debug.WriteLine("Feedback received : ");
            Debug.WriteLine(SelectedFeedback.FeedbackContent);

            ViewModel.SelectedFeedback = SelectedFeedback;

            //showcase information about doctors related to this Feedback
            //send a request to gather information about docors related to a particular Feedback ID
            url = "doctordata/listdoctorssforFeedback/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DoctorDetailDto> RelatedDoctors = response.Content.ReadAsAsync<IEnumerable<DoctorDetailDto>>().Result;

            ViewModel.RelatedDoctors = RelatedDoctors;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Feedback/New
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult New()
        {
            string url = "doctordetailsdata/listdoctordetails";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DoctorDetailDto> DoctorDetail = response.Content.ReadAsAsync<IEnumerable<DoctorDetailDto>>().Result;
            return View(DoctorDetail);
        }

        // POST: Feedback/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
        public ActionResult Create(Feedback Feedback)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Feedback.FeedbackName);
            //objective: add a new Feedback into our system using the API
            //curl -H "Content-Type:application/json" -d @Feedback.json https://localhost:44342/api/Feedbackdata/addFeedback 
            string url = "Feedbackdata/addFeedback";


            string jsonpayload = jss.Serialize(Feedback);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Feedback/Edit/3
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "Feedbackdata/findFeedback/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FeedbackDto selectedFeedback = response.Content.ReadAsAsync<FeedbackDto>().Result;
            return View(selectedFeedback);
        }

        // POST: Feedback/Update/3
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Feedback Feedback)
        {

            string url = "Feedbackdata/updateFeedback/" + id;
            string jsonpayload = jss.Serialize(Feedback);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Feedback/Delete/3
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Feedbackdata/findFeedback/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            FeedbackDto selectedFeedback = response.Content.ReadAsAsync<FeedbackDto>().Result;
            return View(selectedFeedback);
        }

        // POST: Feedback/Delete/3
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Feedbackdata/deleteFeedback/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
