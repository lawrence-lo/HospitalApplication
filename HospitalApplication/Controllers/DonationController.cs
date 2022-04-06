using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using HospitalApplication.Models;
using System.Web.Script.Serialization;
using HospitalApplication.Models.ViewModels;

namespace HospitalApplication.Controllers
{
    public class DonationController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonationController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/api/");
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
        public ActionResult New()
        {
            //information about all the donors in the system
            //GET api/donordata/listdonors
            string url = "donordata/listdonors";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DonorDto> donorOptions = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;

            return View(donorOptions);
        }

        // POST: Donation/Create
        [HttpPost]
        public ActionResult Create(Donation donation)
        {
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

            return View(ViewModel);
        }

        // POST: Donation/Update/5
        [HttpPost]
        public ActionResult Update(int id, Donation donation)
        {
            string url = "donationdata/updatedonation" + id;
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
        public ActionResult DeleteConfirm(int id)
        {
            string url = "donationdata/finddonation" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonationDto SelectedDonation = response.Content.ReadAsAsync<DonationDto>().Result;
            return View(SelectedDonation);
        }

        // POST: Donation/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "donationdata/deletedonation" + id;
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
