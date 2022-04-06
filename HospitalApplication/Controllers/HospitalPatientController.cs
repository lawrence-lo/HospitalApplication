using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalApplication.Models;
using System.Web.Script.Serialization;

namespace HospitalApplication.Controllers
{
    public class HospitalPatientController : Controller
    {

        private static readonly HttpClient client;
        static HospitalPatientController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/api/");
        }

        // GET: HospitalPatient/List
        public ActionResult List()
        {
            //objective: communuicate with HospitalPatient data api to retrieve a list of patients
            //curl: https://localhost:44325/api/HospitalPatientData/ListHospitalPatients
            //HttpClient client = new HttpClient(){ };
            string url = "HospitalPatientData/ListHospitalPatients";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("status is " + response.StatusCode);

            IEnumerable<HospitalPatientDto> hospitalPatients = response.Content.ReadAsAsync<IEnumerable<HospitalPatientDto>>().Result;

            return View(hospitalPatients);
        }


        // GET: HospitalPatient/Details/5
        public ActionResult Details(int id)
        {
            string url = "HospitalPatientData/FindPatients" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HospitalPatientDto selectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;
            return View(selectedPatient);
        }

        // GET: HospitalPatientData/Create
        [HttpPost]
        public ActionResult Create(HospitalPatient hospitalPatient)
        {
           
            string url = "HospitalPatientData/AddNewPatient";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(hospitalPatient);

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

        // POST: HospitalPatient/Create
        [HttpPost]
        public ActionResult CreateOld(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: HospitalPatientData/Edit/5
    
        public ActionResult Edit(int id)
        {
            string url = "HospitalPatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HospitalPatientDto SelectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;

            return View(SelectedPatient);
        }

        // Post: HospitalPatient/Update/5
        public ActionResult Update(int id, HospitalPatient hospitalPatient)
        {
            string url = "HospitalPatientData/UpdateHospitalPatient/" + id;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(hospitalPatient);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);

            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }

        // GET: HospitalPatient/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "HospitalPatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HospitalPatientDto selectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;

            return View(selectedPatient);
        }

        // POST: HospitalPatient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "HospitalPatientData/DeleteHospitalPatient/" + id;
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
