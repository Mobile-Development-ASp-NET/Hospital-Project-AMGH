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
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// For proper WebAPI authentication, you can send a post request with login credentials to the WebAPI and log the access token from the response. The controller already knows this token, so we're just passing it up the chain.
        /// 
        /// Here is a descriptive article which walks through the process of setting up authorization/authentication directly.
        /// https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/individual-accounts-in-web-api
        /// </summary>
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
            //This is a bit dangerous because a previously authenticated cookie could be cached for
            //a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            //collect token as it is submitted to the controller
            //use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }
        /// <summary>
        /// objective: communicate with our Admission data api to retrieve a list of Admissions
        /// curl https://localhost:44342/api/Admissiondata/listAdmissions
        /// </summary>
        /// <returns></returns>
        // GET: Admission/List
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult List()
        {

            GetApplicationCookie();
     
            string url = "Admissiondata/listAdmissions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<AdmissionDto> Admissions = response.Content.ReadAsAsync<IEnumerable<AdmissionDto>>().Result;
            //Debug.WriteLine("Number of Admissions received : ");
            //Debug.WriteLine(Admissions.Count());


            return View(Admissions);
        }

        /// <summary>
        /// objective: communicate with our Admission data api to retrieve one Admission
        /// curl https://localhost:44342/api/Admissiondata/findAdmission/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admission/Details/5
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// GET: Admission/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        /// <summary>
        /// To create an admission in to the database
        /// </summary>
        /// <param name="Admission"></param>
        /// <returns></returns>
        // POST: Admission/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Admission Admission)
        {
            GetApplicationCookie();
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

        /// <summary>
        /// To edit the admission details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admission/Edit/5
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();
            string url = "Admissiondata/findAdmission/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdmissionDto selectedAdmission = response.Content.ReadAsAsync<AdmissionDto>().Result;
            return View(selectedAdmission);
        }

        /// <summary>
        /// To update the admission details into the system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Admission"></param>
        /// <returns></returns>
        // POST: Admission/Update/5
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Update(int id, Admission Admission)
        {
            GetApplicationCookie();
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

        /// <summary>
        /// To confirm the delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: Admission/Delete/5
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();
            string url = "Admissiondata/findAdmission/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            AdmissionDto selectedAdmission = response.Content.ReadAsAsync<AdmissionDto>().Result;
            return View(selectedAdmission);
        }

        // POST: Admission/Delete/5
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
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
