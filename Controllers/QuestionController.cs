using Hospital_Project.Models.ViewModels;
using Hospital_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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
            url = "SurveyData/ListSurveysForQuestions/" + id;
            response = client.GetAsync(url).Result;
            List<SurveyDto> relatedSurveys = response.Content.ReadAsAsync<List<SurveyDto>>().Result;
            ViewModel.relatedSurveys = relatedSurveys;

            return View(ViewModel);
        }

        // GET: Question/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Question/Create
        [HttpPost]
        public ActionResult Create(Questions question)
        {
            //creating an actor json data, turn into a string, then eject it into the database
            string url = "QuestionData/AddQuestions";
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        // POST: Question/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Questions question)
        {
            //Accessing the selected question that we want to update
            string url = "QuestionData/UpdateQuestions/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            QuestionDto selectedquestion = response.Content.ReadAsAsync<QuestionDto>().Result;
            return View(selectedquestion);
        }

        // GET: Question/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "QuestionData/FindQuestion" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            QuestionDto selectedquestion = response.Content.ReadAsAsync<QuestionDto>().Result;
            return View(selectedquestion);
        }

        // POST: Question/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Accessing the DeleteQuestion(int id) method to delete the selected question
            string url = "QuestionData/DeleteQuestions/" + id;
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
