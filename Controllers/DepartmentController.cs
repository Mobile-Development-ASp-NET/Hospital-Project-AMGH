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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
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
            Debug.WriteLine(SelectedDepartment.DepartmentName);
            ViewModel.SelectedDepartment = SelectedDepartment;

            Debug.WriteLine(ViewModel);
            return View(ViewModel);
        }
        //GET: Department/Error
        public ActionResult  Error()
        {
            return View();
        }

        // GET: Department/New
        public ActionResult New()
        {

            return View();
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(Department department)
        {
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
        public ActionResult Edit(int id, Department department)
        {
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
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "DepartmentData/FindDepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            return View(SelectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
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
