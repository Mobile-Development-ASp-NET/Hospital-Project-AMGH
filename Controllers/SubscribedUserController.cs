using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hospital_Project.Controllers
{
    public class SubscribedUserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static SubscribedUserController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44344/api/");
        }

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

        // GET: SubscribedUser/List
        //[Authorize]
        public ActionResult List()
        {
            string url = "subscribeduserdata/listsubscribedusers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<SubscribedUserDto> subscribedUserDto = response.Content.ReadAsAsync<IEnumerable<SubscribedUserDto>>().Result;

            return View(subscribedUserDto);
        }

        // GET: SubscribedUser/Details/5
        public ActionResult Details(int id)
        {
            DetailsSubscribedUser ViewModel = new DetailsSubscribedUser();

            string url = "subscribeduserdata/findsubscribeduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            SubscribedUserDto SelectedSubscribedUser = response.Content.ReadAsAsync<SubscribedUserDto>().Result;

            ViewModel.SelectedSubscribedUser = SelectedSubscribedUser;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: SubscribedUser/New
        public ActionResult New()
        {
            return View();
        }

        // POST: SubscribedUser/Create
        [HttpPost]
        //[Authorize]
        public ActionResult Create(SubscribedUser subscribedUser)
        {
            GetApplicationCookie(); // get token credentials

            string url = "subscribeduserdata/addsubscribeduser";

            string jsonpayload = jss.Serialize(subscribedUser);
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

        // GET: SubscribedUser/Edit/5
        //[Authorize]
        public ActionResult Edit(int id)
        {
            string url = "subscribeduserdata/findsubscribeduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SubscribedUserDto subscribedUserDto = response.Content.ReadAsAsync<SubscribedUserDto>().Result;
            return View(subscribedUserDto);
        }

        // POST: SubscribedUser/Update/5
        [HttpPost]
        //[Authorize]
        public ActionResult Update(int id, SubscribedUser subscribedUser)
        {
            string url = "subscribeduserdata/updatesubscribeduser/" + id;
            string jsonpayload = jss.Serialize(subscribedUser);
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

        // GET: SubscribedUser/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "subscribeduserdata/deletesubscribeduser/" + id;
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

        // POST: SubscribedUser/DeleteConfirmed/5
        [HttpPost]
        //[Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "subscribeduserdata/findsubscribeduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SubscribedUserDto subscribedUserDto = response.Content.ReadAsAsync<SubscribedUserDto>().Result;
            return View(subscribedUserDto);
        }

    }
}