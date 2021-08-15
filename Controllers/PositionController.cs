using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;

namespace Hospital_Project.Controllers
{
    public class PositionController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PositionController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44342/api/");
        }

        // GET: Position/List
        //[Authorize]
        public ActionResult List()
        {
            // objective: communicate with the position data api to retreieve a list of positions
            // curl https://localhost:44342/api/positiondata/listposition

            string url = "positiondata/listpositions";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PositionDto> Positions = response.Content.ReadAsAsync<IEnumerable<PositionDto>>().Result;

            return View(Positions);
        }

        // GET: Position/Details/5
        //[Authorize]
        public ActionResult Details(int id)
        {
            // objective: communicate with the position data api to retrieve one position
            // curl https://localhost:44342/api/positiondata/findposition/{id}

            DetailsPosition ViewModel = new DetailsPosition();

            string url = "positiondata/findposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PositionDto SelectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;

            ViewModel.SelectedPositions = SelectedPosition;

            // showcase information about applications related to this position
            // send a request tp gather information about application related to a particular position ID
            url = "ApplicationData/ListApplicationsForPosition/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<ApplicationDto> RelatedApplications = response.Content.ReadAsAsync<IEnumerable<ApplicationDto>>().Result;
            // Error found when selecting the applications assigned to that position

            ViewModel.RelatedApplication = RelatedApplications;

            // showcase information on positions related to the departments
            url = "PositionData/ListPostionsForDepartment/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> RelatedDepartments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.RelatedDepartment = RelatedDepartments;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: Position/New
        //[Authorize]
        public ActionResult New()
        {
            // Need the positionDto for validation 
            UpdatePosition ViewModel = new UpdatePosition();

            // information about all departments in the system
            // GET api/departmentdata/listdepartments
            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;
            
            return View(ViewModel);
        }

        // POST: Position/Create
        //[Authorize]
        [HttpPost]
        public ActionResult Create(Position Position)
        {
            // objective: add a new position into our system using the api
            // curl -H "Content-Type:application/json" -d @Position.json https://localhost:44342/api/positiondata/addposition
            string url = "positiondata/addposition";

            string jsonpayload = jss.Serialize(Position);

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

        // GET: Position/Edit/5
        //[Authorize]
        public ActionResult Edit(int id)
        {
            UpdatePosition ViewModel = new UpdatePosition();

            // the existing position information
            string url = "positiondata/findposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            PositionDto SelectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;
            ViewModel.SelectedPosition = SelectedPosition;

            // all department to choose from when updating the position
            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;
            
            return View(ViewModel);
        }

        // POST: Position/Edit/5
        //[Authorize]
        [HttpPost]
        public ActionResult Update(int id, Position Position)
        {
            string url = "positiondata/updateposition/" + id;
            string jsonpayload = jss.Serialize(Position);
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

        // GET: Position/Delete/5
        //[Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "positiondata/findposition/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PositionDto SelectedPosition = response.Content.ReadAsAsync<PositionDto>().Result;
            return View(SelectedPosition);
        }

        // POST: Position/Delete/5
        //[Authorize]
        [HttpPost]
        public ActionResult Delete(int id, Position Position)
        {
            string url = "positiondata/deleteposition/" + id;
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
