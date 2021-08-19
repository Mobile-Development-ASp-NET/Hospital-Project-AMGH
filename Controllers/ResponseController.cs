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
    public class ResponseController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ResponseController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44336/api/");
        }


        // GET: Response
        [HttpGet]
        public ActionResult List()
        {
            //Accessing a list of responses from responseData Listresponses() method.
            //curl https://localhost:44336/api/ResponseData/ListResponses
            string url = "ResponseData/ListResponses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            List<ResponseDto> Responses = response.Content.ReadAsAsync<List<ResponseDto>>().Result;

            return View(Responses);
        }

        // GET: Response/Details/5
        public ActionResult Details(int id)
        {
            //Accessing a selected Response info from the Responses table with FindResponse(int id) method
            // curl https://localhost:44336/api/ResponseData/FindResponse /{ id}
            string url = "ResponseData/FindResponse/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ResponseDto selectedResponse = response.Content.ReadAsAsync<ResponseDto>().Result;

            return View(selectedResponse);
        }

        // GET: Response/Create
        public ActionResult New()
        {
            return View();
        }

        // POST: Response/Create
        [HttpPost]
        public ActionResult Create(Response responses)
        {
            string url = "ResponseData/AddResponse";
            string jsonpayload = jss.Serialize(responses);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Response/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Response/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Response/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Response/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
