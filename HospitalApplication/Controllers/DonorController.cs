﻿using System;
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
        public ActionResult Details(int id)
        {
            //objective: communicate with Donor api to retrieve one list of Donors
            //curl https://localhost:44325/api/donordata/finddonor/{id}

            DetailsDonor ViewModel = new DetailsDonor();

            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DonorDto selecteddonor = response.Content.ReadAsAsync<DonorDto>().Result;
            ViewModel.SelectedDonor = selecteddonor;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Donor/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Donor/Create
        [HttpPost]
        public ActionResult Create(Donor donor)
        {
            //curl -H "Content-Type:application/jason" -d @donor.json https://localhost:44325/api/donordata/adddonor
            string url = "donordata/adddonor";
            string jsonpayload = jss.Serialize(donor);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Donor/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateDonor ViewModel = new UpdateDonor();

            string url = "donordata/finddonor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto selecteddonor = response.Content.ReadAsAsync<DonorDto>().Result;
            ViewModel.SelectedDonor = selecteddonor;

            return View(ViewModel);
        }

        // POST: Donor/Update/5
        [HttpPost]
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
        public ActionResult DeleteConfirm(int id)
        {
            string url = "donordata/finddonor" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DonorDto selecteddonor = response.Content.ReadAsAsync<DonorDto>().Result;
            return View(selecteddonor);
        }

        // POST: Donor/Delete/5
        [HttpPost]
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
