using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Mvc;
using Hospital_Project.Models;
using System.Web.Script.Serialization;
using Hospital_Project.Models.ViewModels;

namespace Hospital_Project.Controllers
{
    public class GreetingCardController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static GreetingCardController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
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

        // GET: GreetingCard/List
        public ActionResult List()
        {
            //objective: communicate with our greeting card data api to retrieve a list of greeting cards
            //curl https://localhost:44342/api/greetingcarddata/listgreetingcards


            string url = "greetingcarddata/listgreetingcards";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<GreetingCardDto> GreetingCards = response.Content.ReadAsAsync<IEnumerable<GreetingCardDto>>().Result;
            //Debug.WriteLine("Number of GreetingCards received : ");
            //Debug.WriteLine(GreetingCards.Count());


            return View(GreetingCards);
        }

        // GET: GreetingCard/Details/5
        public ActionResult Details(int id)
        {
            GreetingCardDetails ViewModel = new GreetingCardDetails();

            //objective: communicate with our GreetingCard data api to retrieve one GreetingCard
            //curl https://localhost:44342/api/greetingcarddata/findgreetingcard/{id}

            string url = "greetingcarddata/findgreetingcard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            GreetingCardDto SelectedCard = response.Content.ReadAsAsync<GreetingCardDto>().Result;
            Debug.WriteLine("GreetingCard received : ");
            Debug.WriteLine(SelectedCard.SenderFirstName);

            ViewModel.SelectedCard = SelectedCard;

            return View(ViewModel);
        }

        public ActionResult Error()
        {

            return View();
        }

        // GET: GreetingCard/New
        [Authorize]
        public ActionResult New()
        {
            //information about all Admissions in the system.
            //GET api/Admissionsdata/listAdmissions

            string url = "admissiondata/listadmissions";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AdmissionDto> AdmissionOptions = response.Content.ReadAsAsync<IEnumerable<AdmissionDto>>().Result;
            return View(AdmissionOptions);
        }

        // POST: GreetingCard/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(GreetingCard GreetingCard)
        {
            GetApplicationCookie();//get token credentials
            Debug.WriteLine("the json payload is :");
            //Debug.WriteLine(GreetingCard.GreetingCardName);
            //objective: add a new GreetingCard into our system using the API
            //curl -H "Content-Type:application/json" -d @GreetingCard.json https://localhost:44342/api/GreetingCarddata/addGreetingCard 
            string url = "GreetingCarddata/addGreetingCard";


            string jsonpayload = jss.Serialize(GreetingCard);
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

        // GET: GreetingCard/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateGreetingCard ViewModel = new UpdateGreetingCard();

            //the existing GreetingCard information
            string url = "GreetingCarddata/findGreetingCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GreetingCardDto SelectedGreetingCard = response.Content.ReadAsAsync<GreetingCardDto>().Result;
            ViewModel.SelectedCard = SelectedGreetingCard;

            // all Admissions to choose from when updating this GreetingCard
            //the existing GreetingCard information
            url = "admissiondata/listadmissions/";
            response = client.GetAsync(url).Result;
            IEnumerable<AdmissionDto> AdmissionOptions = response.Content.ReadAsAsync<IEnumerable<AdmissionDto>>().Result;

            ViewModel.AdmissionOptions = AdmissionOptions;

            return View(ViewModel);
        }

        // POST: GreetingCard/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, GreetingCard GreetingCard, HttpPostedFileBase GreetingCardPic)
        {
            GetApplicationCookie();//get token credentials   
            string url = "GreetingCarddata/updateGreetingCard/" + id;
            string jsonpayload = jss.Serialize(GreetingCard);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && GreetingCardPic != null)
            {
                //Updating the GreetingCard picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "greetingcarddata/uploadgreetingcardpic/" + id;
                //Debug.WriteLine("Received GreetingCard Picture "+GreetingCardPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(GreetingCardPic.InputStream);
                requestcontent.Add(imagecontent, "GreetingCardPic", GreetingCardPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: GreetingCard/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "GreetingCarddata/findGreetingCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GreetingCardDto selectedGreetingCard = response.Content.ReadAsAsync<GreetingCardDto>().Result;
            return View(selectedGreetingCard);
        }

        // POST: GreetingCard/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "GreetingCarddata/deleteGreetingCard/" + id;
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
