using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hospital_Project.Controllers
{
    public class SurveyController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SurveyController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        // GET: Survey
        public ActionResult List()
        {
            //Accessing a list of Surveys from  ListSurveys() Method
            string url = "SurveyData/ListSurveys";
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<SurveyDto> surveys = response.Content.ReadAsAsync<List<SurveyDto>>().Result;
            return View(surveys);
        }

        // GET: Survey/Details/5
        public ActionResult Details(int id)
        {
            //Accessing a selected survey info from the surveys table with Findsurvey(int id) method
            DetailsSurvey ViewModel = new DetailsSurvey();
            string url = "SurveyData/FindSurvey/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SurveyDto selectedsurvey = response.Content.ReadAsAsync<SurveyDto>().Result;
            ViewModel.selectedSurvey = selectedsurvey;

            //show all questions that are related to one survey
            url = "questionData/ListQuestionsForSurvey/" + id;
            response = client.GetAsync(url).Result;
            List<QuestionDto> relatedQuestions = response.Content.ReadAsAsync<List<QuestionDto>>().Result;
            ViewModel.relatedquestions = relatedQuestions;

            return View(ViewModel);
        }

        // GET: Survey/Create
        [Authorize(Roles ="Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Survey/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Survey survey)
        {
            GetApplicationCookie();
            //creating an survey json data, turn into a string, then eject it into the database
            string url = "SurveyData/AddSurvey";
            string jsonpayload = jss.Serialize(survey);
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

        public ActionResult Error()
        {
            return View();
        }

        // GET: Survey/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            //Accessing the selected survey that we want to update
            string url = "SurveyData/FindSurvey/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SurveyDto selectedsurvey = response.Content.ReadAsAsync<SurveyDto>().Result;
            return View(selectedsurvey);
        }

        // POST: Survey/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Survey survey)
        {
            GetApplicationCookie();
            //Use the UpdateSurveys(int id) Method to update the selected Survey's information
            string url = "SurveyData/UpdateSurvey/" + id;
            string jsonpayload = jss.Serialize(survey);
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

        // GET: Survey/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "SurveyData/FindSurvey/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SurveyDto selectedsurvey = response.Content.ReadAsAsync<SurveyDto>().Result;
            return View(selectedsurvey);
        }

        // POST: Survey/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            //Accessing the DeleteSurvey(int id) method to delete the selected Survey
            string url = "SurveyData/DeleteSurvey/" + id;
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
