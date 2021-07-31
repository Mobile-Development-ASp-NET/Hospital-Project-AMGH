using Hospital_Project.Models;
using Hospital_Project.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Hospital_Project.Controllers
{
    public class BlogController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BlogController()
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

        // GET: Blog/List
        public ActionResult List()
        {
            string url = "blogdata/listblogs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<BlogDto> BlogDtos = response.Content.ReadAsAsync<IEnumerable<BlogDto>>().Result;

            return View(BlogDtos);
        }

        // GET: Blog/Details/5
        public ActionResult Details(int id)
        {
            DetailsBlog ViewModel = new DetailsBlog();

            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            BlogDto SelectedBlog = response.Content.ReadAsAsync<BlogDto>().Result;

            ViewModel.SelectedBlog = SelectedBlog;

            url = "subscribeduserdata/listsubscribedusersforblog/" + id;
            response = client.GetAsync(url).Result;

            IEnumerable<SubscribedUserDto> SubscribedUsers = response.Content.ReadAsAsync<IEnumerable<SubscribedUserDto>>().Result;

            ViewModel.ApprovedUsers = SubscribedUsers;

            return View(ViewModel);
        }

        // GET: Blog/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Blog/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        //[Authorize]
        public ActionResult Create( Blog blog)
        {
            GetApplicationCookie(); // get token credentials

            string url = "blogdata/addblog";

            string jsonpayload = jss.Serialize(blog);
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

        // GET: Blog/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BlogDto blogDto = response.Content.ReadAsAsync<BlogDto>().Result;
            return View(blogDto);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        //[Authorize]
        public ActionResult Update(int id, Blog blog)
        {
            string url = "blogdata/updateblog/" + id;
            string jsonpayload = jss.Serialize(blog);
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

        // GET: Blog/Delete/5
        public ActionResult Delete(int id)
        {
            string url = "blogdata/deleteblog/" + id;
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

        // POST: Blog/DeleteConfirmed/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            string url = "blogdata/findblog/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BlogDto blogDto = response.Content.ReadAsAsync<BlogDto>().Result;
            return View(blogDto);
        }
    }
}