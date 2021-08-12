using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Script.Serialization;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;

namespace Hospital_Project.Controllers
{

    public class DoctorDetailController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static DoctorDetailController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
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
        public ActionResult New()
        {
            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            return View(departments);
        }

        // POST: DoctorDetail/Create
        [HttpPost]
        public ActionResult Create(DoctorDetails doctorDetails)
        {
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
        public ActionResult Edit(int id, DoctorDetails doctorDetails)
        {
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
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "DoctorDetailsData/FindDoctorDetail/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DoctorDetailDto selectedDr = response.Content.ReadAsAsync<DoctorDetailDto>().Result;
            return View(selectedDr);
        }

        // POST: DoctorDetail/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
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
