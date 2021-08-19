using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System.Diagnostics;

namespace Hospital_Project.Controllers
{

    public class DoctorDetailController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DoctorDetailController()
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


        // GET: List
        public ActionResult List()
        {
            string url = "doctordetailsData/ListDoctorDetails";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DoctorDetailDto> doctorDetails = response.Content.ReadAsAsync<IEnumerable<DoctorDetailDto>>().Result;
            return View(doctorDetails);
        }

        // GET: DoctorDetail/Details/5
        public ActionResult Details(int id)
        {
            string url = "doctordetailsData/FindDoctorDetail/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DoctorDetailDto doctorDetail = response.Content.ReadAsAsync<DoctorDetailDto>().Result;
            return View(doctorDetail);
        }

        //GET :DoctorDetail/Error

        public ActionResult Error()
        {
            return View();
        }
        // GET: DoctorDetail/New
        [Authorize(Roles ="Admin")]
        public ActionResult New()
        {

            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // POST: DoctorDetail/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(DoctorDetails doctorDetails)
        {
            GetApplicationCookie();//get token credentials
            string url = "doctorDetailsData/addDoctor";
            //objective: add a new  doctor.
            string jsonpayload = jss.Serialize(doctorDetails);

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

        // GET: DoctorDetail/Update/5
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {
            UpdateDoctorDetails viewModel = new UpdateDoctorDetails();

            // Objective: Commmunicate with the DoctorDetail data api  to retrive a specific doctor details with id.
            // curl api/DoctorDetailsData/FindDoctorDetail/1

            string url = "DoctorDetailsData/FindDoctorDetail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DoctorDetailDto selectedDr = response.Content.ReadAsAsync<DoctorDetailDto>().Result;
            viewModel.selectedDoctor = selectedDr;

            //information about all departments 
            //GET api/departmentdata/listdepartments

            url = "departmentdata/listdepartments";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            viewModel.departmentOptions = departments;


            return View(viewModel);
        }

        // POST: DoctorDetail/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, DoctorDetails doctorDetails)
        {
            GetApplicationCookie();//get token credentials
            string url = "doctorDetailsData/UpdateDoctorDetail/"+id;
            //objective: update a new  doctor.
            doctorDetails.DrId = id;
            string jsonpayload = jss.Serialize(doctorDetails);

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

        // GET: DoctorDetail/DeleteConfirmation/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "DoctorDetailsData/FindDoctorDetail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DoctorDetailDto selectedDr = response.Content.ReadAsAsync<DoctorDetailDto>().Result;
            return View(selectedDr);
        }

        // POST: DoctorDetail/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "doctorDetailsData/DeleteDoctorDetail/" + id;
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
