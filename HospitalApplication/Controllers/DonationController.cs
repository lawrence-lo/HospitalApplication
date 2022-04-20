using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using HospitalApplication.Models;
using System.Web.Script.Serialization;
using HospitalApplication.Models.ViewModels;
using System.Diagnostics;

namespace HospitalApplication.Controllers
{
    public class DonationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonationController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44325/api/");
        }
        private void GetApplicationCookie()
        {
            string token = "";
            //HTTP client is set up to be reused, otherwise it will exhaust server resources.
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

        // GET: Donation/List
        public ActionResult List()
        {
            //objective: communicate with Donation api to retrieve a list of Donations
            //curl https://localhost:44325/api/donationdata/listdonations

            string url = "donationdata/listdonations";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DonationDto> donations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;

            return View(donations);
        }

        // GET: Donation/Details/5
        public ActionResult Details(int id)
        {
            DetailsDonation ViewModel = new DetailsDonation();

            //objective: communicate with our donation data api to retrieve one donation
            //curl https://localhost:44325/api/postdata/findpost/{id}

            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;

            ViewModel.SelectedDonation = SelectedDonation;

            //Todo: put user info in viewmodel

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Donation/New
        [Authorize(Roles = "Admin")]

        public ActionResult New()
        {
            NewDonation ViewModel = new NewDonation();

            //information about all the donors in the system
            //GET api/donordata/listdonors
            string url = "";
            HttpResponseMessage response = client.GetAsync(url).Result;

            url = "donordata/listdonors/";
            response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> DonorOptions = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            ViewModel.DonorOptions = DonorOptions;

            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Donation/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Create(Donation donation)
        {
            GetApplicationCookie();
            //objective: add a new donation into our system using the API
            //curl -H "Content-Type:application/json" -d @donation.json https://localhost:44325/api/donationdata/adddonation
            string url = "donationdata/adddonation";

            string jsonpayload = jss.Serialize(donation);

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

        // GET: Donation/Edit/5
        [Authorize(Roles = "Admin")]

        public ActionResult Edit(int id)
        {
            UpdateDonation ViewModel = new UpdateDonation();

            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
            ViewModel.SelectedDonation = SelectedDonation;

            //all donors to choose from when updating this Donation
            url = "donordata/listdonors/";
            response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> DonorOptions = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;
            ViewModel.DonorOptions = DonorOptions;

            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Donation/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Update(int id, Donation donation)
        {
            GetApplicationCookie();

            string url = "donationdata/updatedonation/" + id;
            string jsonpayload = jss.Serialize(donation);

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

        // GET: Donation/Delete/5
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "donationdata/finddonation/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
            return View(SelectedDonation);
        }

        // POST: Donation/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]

        public ActionResult Delete(int id)
        {
            GetApplicationCookie();

            string url = "donationdata/deletedonation/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            Debug.WriteLine(id);
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
