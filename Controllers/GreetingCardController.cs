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
    /// <summary>
    /// A controller for the greeting card to connect the datacontroller to the view
    /// </summary>
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

        // GET: GreetingCard/List?=PageNum = {PageNum}
        /// <summary>
        /// To list out all the greeting cards( to connect it to the list method and the list view)
        /// </summary>
        /// <param name="PageNum"></param>
        /// <returns></returns>
        /// objective: communicate with our greeting card data api to retrieve a list of greeting cards
        ///curl https://localhost:44342/api/greetingcarddata/listgreetingcards
        [Authorize (Roles ="Admin,Guest")]
        public ActionResult List(int PageNum=0)
        {

            GetApplicationCookie();
            GreetingCardList ViewModel = new GreetingCardList();
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin")) ViewModel.IsAdmin = true;
            else ViewModel.IsAdmin = false;

            string url = "greetingcarddata/listgreetingcards";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<GreetingCardDto> GreetingCards = response.Content.ReadAsAsync<IEnumerable<GreetingCardDto>>().Result;
            //Debug.WriteLine("Number of GreetingCards received : ");
            //Debug.WriteLine(GreetingCards.Count());

            // -- Start of Pagination Algorithm --

            // Find the total number of greeting cards
            int GreetingCardCount = GreetingCards.Count();
            // Number of cards to display per page
            int PerPage = 4;
            // Determines the maximum number of pages (rounded up), assuming a page 0 start.
            int MaxPage = (int)Math.Ceiling((decimal)GreetingCardCount / PerPage) - 1;

            // Lower boundary for Max Page
            if (MaxPage < 0) MaxPage = 0;
            // Lower boundary for Page Number
            if (PageNum < 0) PageNum = 0;
            // Upper Bound for Page Number
            if (PageNum > MaxPage) PageNum = MaxPage;

            // The Record Index of our Page Start
            int StartIndex = PerPage * PageNum;

            //Helps us generate the HTML which shows "Page 1 of ..." on the list view
            ViewData["PageNum"] = PageNum;
            ViewData["PageSummary"] = " " + (PageNum + 1) + " of " + (MaxPage + 1) + " ";

            // -- End of Pagination Algorithm --

            //Send another request to get the page slice of the full list
            url = "GreetingCardData/ListGreetingCardsPage/" + StartIndex + "/" + PerPage;
            response = client.GetAsync(url).Result;

            // Retrieve the response of the HTTP Request
            IEnumerable<GreetingCardDto> SelectedGreetingCardsPage = response.Content.ReadAsAsync<IEnumerable<GreetingCardDto>>().Result;

            ViewModel.GreetingCards = SelectedGreetingCardsPage;



            return View(ViewModel);
        }
        /// <summary>
        /// objective: communicate with our GreetingCard data api to retrieve one GreetingCard
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// GET: GreetingCard/Details/5
        /// curl https://localhost:44342/api/greetingcarddata/findgreetingcard/{id}
        [Authorize(Roles ="Admin, Guest")]
        public ActionResult Details(int id)
        {
            GetApplicationCookie();
            GreetingCardDetails ViewModel = new GreetingCardDetails();

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
        /// <summary>
        /// to view the custom error messages
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {

            return View();
        }
        /// <summary>
        /// to create new greeting cards (generate all the admissions in the system to choose from)
        /// </summary>
        /// <returns></returns>
        // GET: GreetingCard/New
        [Authorize(Roles = "Admin, Guest")]
        public ActionResult New()
        {
            //need the greeting card DTO for validation
            UpdateGreetingCard ViewModel = new UpdateGreetingCard();

            //information about all Admissions in the system.
            //GET api/Admissionsdata/listAdmissions

            string url = "admissiondata/listadmissions";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<AdmissionDto> AdmissionOptions = response.Content.ReadAsAsync<IEnumerable<AdmissionDto>>().Result;
            return View(AdmissionOptions);
        }
        /// <summary>
        /// create new greeting card
        /// </summary>
        /// <param name="GreetingCard"></param>
        /// <returns></returns>
        // POST: GreetingCard/Create
        [HttpPost]
        [Authorize(Roles = "Admin,Guest")]
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

        /// <summary>
        /// to update the greeting cards The update functionality is only for admin.
        /// If the user wants to update they can reach out the admin for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: GreetingCard/Edit/5
        [Authorize(Roles ="Admin,Guest")]
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

        /// <summary>
        /// to update the greeting card
        /// </summary>
        /// <param name="id"></param>
        /// <param name="GreetingCard"></param>
        /// <param name="GreetingCardPic"></param>
        /// <returns></returns>
        // POST: GreetingCard/Update/5
        [HttpPost]
        [Authorize(Roles ="Admin,Guest")]
        public ActionResult Update(int id, GreetingCard GreetingCard, HttpPostedFileBase GreetingCardPic)
        {
            GetApplicationCookie();//get token credentials   
            string url = "GreetingCardData/UpdateGreetingCard/" + id;
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
                url = "GreetingCardData/UploadGreetingCardPic/" + id;
                Debug.WriteLine("Received GreetingCard Picture "+GreetingCardPic.FileName);

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
        /// <summary>
        /// to delete the greeting card a confirmation message
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: GreetingCard/Delete/5
        [Authorize (Roles ="Admin, Guest")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "GreetingCarddata/findGreetingCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            GreetingCardDto selectedGreetingCard = response.Content.ReadAsAsync<GreetingCardDto>().Result;
            return View(selectedGreetingCard);
        }

        /// <summary>
        /// delete the greeting card from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // POST: GreetingCard/Delete/5
        [HttpPost]
        [Authorize(Roles ="Admin , Guest")]
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
