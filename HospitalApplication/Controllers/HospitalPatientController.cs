using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalApplication.Models;
using System.Web.Script.Serialization;
using HospitalApplication.Models.ViewModels;

namespace HospitalApplication.Controllers
{
    public class HospitalPatientController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();


        static HospitalPatientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                //cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44325/api/");
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
            if (cookie != null) token = cookie.Value;

            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
        }


        // ----- [C] CREATE SECTION -----------------------------------------------------------------------------

        // GET: HospitalPatient/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }


        // POST: HospitalPatientData/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(HospitalPatient hospitalPatient)
        {
            Debug.WriteLine("---- create controller----");

            GetApplicationCookie();//get token credentials

            //objective: add a new patient into our system using the API
            //curl -H "Content-Type:application/json" -d @post.json https://localhost:44325/api/HospitalPatientdata/AddNewPatient 
            string url = "HospitalPatientData/AddNewPatient";
         
            string jsonpayload = jss.Serialize(hospitalPatient);
         
            Debug.WriteLine("----------");
            Debug.WriteLine(jsonpayload);


            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine("----------response " + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        // ----- [R] READ SECTION -----------------------------------------------------------------------------

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
            Debug.WriteLine(" -------- in DETAILS function of P Controller ");
            DetailsPatient ViewModel = new DetailsPatient();

            //objective: communicate with our donation data api to retrieve one donation
            //curl https://localhost:44325/api/HospitalPatientData/ListHospitalPatients/findHospitalPatient/{id}

            string url = "HospitalPatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HospitalPatientDto SelectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;

            ViewModel.SelectedPatient = SelectedPatient;



            // get all appointments of this patient

            Debug.WriteLine(" -------- passing id to url  " + SelectedPatient.PatientID);
            url = "HospitalPatientData/FindPatientAppointments/" + SelectedPatient.PatientID;
            HttpResponseMessage patientAppointments = client.GetAsync(url).Result;

            ViewModel.RelatedAppointments = patientAppointments.Content.ReadAsAsync<IEnumerable<HospitalAppointmentDto>>().Result;

            return View(ViewModel);
        }

        // ----- [U] UPDATE SECTION -----------------------------------------------------------------------------

        // GET: HospitalPatientData/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "HospitalPatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HospitalPatientDto SelectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;

            return View(SelectedPatient);

        }

        // Post: HospitalPatient/Update/5
        [HttpPost]
        public ActionResult Update(int id, HospitalPatient hospitalPatient)
        {
            string url = "HospitalPatientData/UpdateHospitalPatient/" + id;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(hospitalPatient);

            Debug.WriteLine("-----"+ jsonpayload);

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

        // ----- [D] DELETE  SECTION -----------------------------------------------------------------------------

        // GET: HospitalPatient/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "HospitalPatientData/FindPatient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HospitalPatientDto selectedPatient = response.Content.ReadAsAsync<HospitalPatientDto>().Result;

            return View(selectedPatient);
        }

       
        // POST: HospitalPatient/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            Debug.WriteLine("----------IN _   delete function of Controller plus passed id " + id);
            //get token
            GetApplicationCookie();

            string url = "HospitalPatientData/DeleteHospitalPatient/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            Debug.WriteLine(" rwsponse is ---" + response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }

        }


        public ActionResult Error()
        {
            return View();
        }
    }
}
