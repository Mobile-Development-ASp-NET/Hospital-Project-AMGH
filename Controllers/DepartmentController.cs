using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using Hospital_Project.Models;
using System.Diagnostics;
using Hospital_Project.Models.ViewModels;


namespace Hospital_Project.Controllers
{
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DepartmentController()
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


        // GET: Department
        public ActionResult List()
        {
            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine(response);
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }
       
        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            Debug.WriteLine(id);
            DetailsDepartment ViewModel = new DetailsDepartment();

            //objective: communicate with our department data api to retrieve one department
            // curl https://localhost:44342/api/applicationdata/findapplication/{id}

            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            // Debug.WriteLine(SelectedDepartment.DepartmentName);
            ViewModel.SelectedDepartment = SelectedDepartment;

            //Debug.WriteLine(ViewModel);
            url = "DoctorDetailsData/ListDoctorDetailForDepartment/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<DoctorDetailDto> associatedDoctors = response.Content.ReadAsAsync<IEnumerable<DoctorDetailDto>>().Result;
            ViewModel.AssociatedDoctors = associatedDoctors;
            return View(ViewModel);
        }
        //GET: Department/Error
        public ActionResult  Error()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        // GET: Department/New
        public ActionResult New()
        {

            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public ActionResult Create(Department department)
        {
            GetApplicationCookie();//get token credentials
            string url = "departmentdata/AddDepartment";
            //objective: add a new  department.
            // curl -d @department.json -H "Content-type:application/json" http://localhost:44342/api/DepartmentData/AddDepartment
            string jsonpayload = jss.Serialize(department);

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

        // GET: Department/Update/5
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id)
        {

            //objective: communicate with our department data api to retrieve one department
            // curl https://localhost:44342/api/applicationdata/findapplication/{id}

            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(SelectedDepartment);


        }

        // POST: Department/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id, Department department)
        {
            GetApplicationCookie();//get token credentials   
            string url = "departmentData/UpdateDepartment/" + id;
            department.DepartmentID = id;
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/DeleteConfirmation/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(SelectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials   
            string url = "departmentdata/deleteDepartment/" + id;
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
