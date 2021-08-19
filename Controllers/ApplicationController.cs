using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;

namespace Hospital_Project.Controllers
{

    public class ApplicationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static ApplicationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }
        // Note on Authentication
        // Since this page contains information that users and non-login users should not view
        // all information except to CREATE an application has been set authorize only to the admin role
        // The CREATE application is available to all.

        /// <summary>
        /// Grabs the authentication cookie sent to this controller.
        /// </summary> 
        private void GetApplicationCookie()
        {
            string token = "";
            
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies.Get(".AspNet.ApplicationCookie");

            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }

        // Get: Appication/List
        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            GetApplicationCookie();//get token credentials

            //objective: commuincate with out application data api to retrieve a list of applications
            //curl https://localhost:44342/api/applicationdata/listapplications

            string url = "applicationdata/listapplications";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<ApplicationDto> applications = response.Content.ReadAsAsync<IEnumerable<ApplicationDto>>().Result;

            return View(applications);

        }

        // GET: Application/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();//get token credentials

            DetailsApplication ViewModel = new DetailsApplication();

            //objective: communicate with our application data api to retrieve one application
            // curl https://localhost:44342/api/applicationdata/findapplication/{id}

            string url = "applicationdata/findapplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;

            ViewModel.SelectedApplication = SelectedApplication;
            
            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // Get: Application/New
        public ActionResult New()
        {
            GetApplicationCookie();//get token credentials

            // information about all positions in the system
            // GET api/positiondata/listpositions

            // Need the applicationdto for validation
            UpdateApplication ViewModel = new UpdateApplication();

            // information on all positions in the system
            string url = "positiondata/listpositions";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PositionDto> PositionsOptions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;

            ViewModel.PositionOptions = PositionsOptions;

            return View(ViewModel);
        }

        // GET: Application/Create
        // No authorize is set since we want the non-login/login users to all create applications
        [HttpPost]
        public ActionResult Create(Application application)
        {
            GetApplicationCookie();//get token credentials

            // objective: add a new application into our system using the api.
            // curl -H "Content-Type:application/json" -d @application.json https://localhost:44342/api/applicationdata/
            string url = "applicationdata/addapplication";

            string jsonpayload = jss.Serialize(application);
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

        // ERROR WHEN UPDATING THE APPLICATION STATUS
        // GET: Application/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie();//get token credentials

            UpdateApplication ViewModel = new UpdateApplication();

            // the existing application information
            string url = "applicationdata/findapplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
            ViewModel.SelectedApplication = SelectedApplication;

            // all positions to choose from when updating the application
            url = "positiondata/listpositions/";
            response = client.GetAsync(url).Result;
            IEnumerable<PositionDto> PositionOptions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;

            ViewModel.PositionOptions = PositionOptions;


            return View(ViewModel);
        }

        // POST: Application/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Application application)
        {
            GetApplicationCookie();//get token credentials

            string url = "applicationdata/updateapplication/" + id;
            string jsonpayload = jss.Serialize(application);
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

        // GET: Application/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie();//get token credentials

            string url = "applicationdata/findapplication/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ApplicationDto SelectedApplication = response.Content.ReadAsAsync<ApplicationDto>().Result;
            return View(SelectedApplication);
        }

        // POST: Application/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials

            string url = "applicationdata/deleteapplication/" + id;
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
