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
    public class DonorController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DonorController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/api/");
        }

        // GET: Donor/List
        public ActionResult List()
        {
            //objective: communicate with Donor api to retrieve a list of Donors
            //curl https://localhost:44325/api/donordata/listdonors

            string url = "donordata/listdonors";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DonorDto> donors = response.Content.ReadAsAsync<IEnumerable<DonorDto>>().Result;

            return View(donors);
        }

        // GET: Donor/Details/5
        [Authorize(Roles="Admin")]
        public ActionResult Details(int id)
        {
            DetailsDonor ViewModel = new DetailsDonor();

            //objective: communicate with our Donor data api to retrieve one donor
            //curl https://localhost:44325/api/donordata/finddonor/{id}

            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;

            ViewModel.SelectedDonor = SelectedDonor;

            //Showcase information about the donation related to this Donor

            url = "donationdata/listdonationsfordonor/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<DonationDto> RelatedDonations = response.Content.ReadAsAsync<IEnumerable<DonationDto>>().Result;
            ViewModel.RelatedDonations = RelatedDonations;
            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Donor/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Donor donor)
        {
            //objective: add a new donor into our system using the API
            //curl -H "Content-Type:application/json" -d @donor.json https://localhost:44325/api/donordata/adddonor
            string url = "donordata/adddonor";

            string jsonpayload = jss.Serialize(donor);

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

        // GET: Donor/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            //UpdateDonor ViewModel = new UpdateDonor();

            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;
            //ViewModel.SelectedDonor = SelectedDonor;

            return View(SelectedDonor);
        }

        // POST: Donor/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Donor donor)
        {
            string url = "donordata/updatedonor"+id;
            string jsonpayload = jss.Serialize(donor);

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

        // GET: Donor/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "donordata/finddonor" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto SelectedDonor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(SelectedDonor);
        }

        // POST: Donor/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "donordata/deletedonor" + id;
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
