using Hospital_Project.Models.ViewModels;
using Hospital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace Hospital_Project.Controllers
{
    public class QuestionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static QuestionController()
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



        // GET: Question
        public ActionResult List()
        {
            //Accessing a list of questions from  ListQuestions() Method
            string url = "QuestionData/ListQuestions";
            HttpResponseMessage response = client.GetAsync(url).Result;
            List<QuestionDto> questions = response.Content.ReadAsAsync<List<QuestionDto>>().Result;
            return View(questions);
        }

        // GET: Question/Details/5
        public ActionResult Details(int id)
        {
            //Accessing a selected question info from the questions table with Findquestion(int id) method
            DetailsQuestion ViewModel = new DetailsQuestion();
            string url = "  QuestionData/FindQuestion/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            QuestionDto selectedquestion = response.Content.ReadAsAsync<QuestionDto>().Result;
            ViewModel.selectedQuestion = selectedquestion;

            //show all surveys that are related to one question
            url = "SurveyData/ListSurveysForQuestion/" + id;
            response = client.GetAsync(url).Result;
            List<SurveyDto> relatedSurveys = response.Content.ReadAsAsync<List<SurveyDto>>().Result;
            ViewModel.relatedSurveys = relatedSurveys;

            return View(ViewModel);
        }

        // GET: Question/Create
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Question question)
        {
            GetApplicationCookie();
            //creating an actor json data, turn into a string, then eject it into the database
            string url = "QuestionData/AddQuestion";
            string jsonpayload = jss.Serialize(question);
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

        // GET: Question/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            //Accessing the selected question that we want to update
            string url = "QuestionData/FindQuestion/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            QuestionDto selectedquestion = response.Content.ReadAsAsync<QuestionDto>().Result;
            return View(selectedquestion);
        }

        public ActionResult Error()
        {
            return View();
        }

        // POST: Question/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Question question)
        {
            GetApplicationCookie();
            //Use the UpdateSurveys(int id) Method to update the selected Survey's information
            string url = "QuestionData/UpdateQuestion/" + id;
            string jsonpayload = jss.Serialize(question);
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

        // GET: Question/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "QuestionData/FindQuestion" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            QuestionDto selectedquestion = response.Content.ReadAsAsync<QuestionDto>().Result;
            return View(selectedquestion);
        }

        // POST: Question/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            //Accessing the DeleteQuestion(int id) method to delete the selected question
            string url = "QuestionData/DeleteQuestion/" + id;
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
