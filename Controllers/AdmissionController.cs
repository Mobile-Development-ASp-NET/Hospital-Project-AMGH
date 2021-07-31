using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Diagnostics;
using System.Net.Http;

namespace Hospital_Project.Controllers
{
    public class AdmissionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static AdmissionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }

        // GET: Admission/List
        public ActionResult List()
        {
            //objective: communicate with our Admission data api to retrieve a list of Admissions
            //curl https://localhost:44342/api/Admissiondata/listAdmissions


            string url = "Admissiondata/listAdmission";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AdmissionDto> Admissions = response.Content.ReadAsAsync<IEnumerable<AdmissionDto>>().Result;
            //Debug.WriteLine("Number of Admissions received : ");
            //Debug.WriteLine(Admissions.Count());


            return View(Admissions);
        }

        // GET: Admission/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our Admission data api to retrieve one Admission
            //curl https://localhost:44342/api/Admissiondata/findAdmission/{id}

            AdmissionDetails ViewModel = new AdmissionDetails();

            string url = "Admissiondata/findAdmission/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            AdmissionDto SelectedAdmission = response.Content.ReadAsAsync<AdmissionDto>().Result;
            Debug.WriteLine("Admission received : ");
            Debug.WriteLine(SelectedAdmission.Room);

            ViewModel.SelectedAdmission = SelectedAdmission;

            //showcase information about cards related to this Admission
            //send a request to gather information about cards related to a particular Admission ID
            url = "greetingcarddata/listgreetingcardsforAdmission/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<GreetingCardDto> RelatedCards = response.Content.ReadAsAsync<IEnumerable<GreetingCardDto>>().Result;

            ViewModel.RelatedCards = RelatedCards;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Admission/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Admission/Create
        [HttpPost]
        public ActionResult Create(Admission Admission)
        {
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(Admission.AdmissionName);
            //objective: add a new Admission into our system using the API
            //curl -H "Content-Type:application/json" -d @Admission.json https://localhost:44342/api/Admissiondata/addAdmission 
            string url = "Admissiondata/addAdmission";


            string jsonpayload = jss.Serialize(Admission);
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

        // GET: Admission/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Admissiondata/findAdmission/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdmissionDto selectedAdmission = response.Content.ReadAsAsync<AdmissionDto>().Result;
            return View(selectedAdmission);
        }

        // POST: Admission/Update/5
        [HttpPost]
        public ActionResult Update(int id, Admission Admission)
        {

            string url = "Admissiondata/updateAdmission/" + id;
            string jsonpayload = jss.Serialize(Admission);
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

        // GET: Admission/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "Admissiondata/findAdmission/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdmissionDto selectedAdmission = response.Content.ReadAsAsync<AdmissionDto>().Result;
            return View(selectedAdmission);
        }

        // POST: Admission/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "Admissiondata/deleteAdmission/" + id;
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
