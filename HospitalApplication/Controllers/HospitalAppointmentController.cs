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
    public class HospitalAppointmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();



        static HospitalAppointmentController()
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

        // GET: HospitalAppointment/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }


        // POST: HospitalAppointment/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(HospitalAppointment hospitalAppointment)
        {

            string url = "HospitalAppointmentData/AddNewAppointment";
         
            string jsonpayload = jss.Serialize(hospitalAppointment);

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

        // ----- [R] READ SECTION -----------------------------------------------------------------------------
       
        // Get HospitalAppointment/List
        public ActionResult List()
        {
            Debug.WriteLine(" App Controller - List ");
            //objective: communuicate with HospitalPatient data api to retrieve a list of patients
            //curl: https://localhost:44325/api/HospitalAppointmentData/ListHospitalAppointments
            //HttpClient client = new HttpClient(){ };
            string url = "HospitalAppointmentData/ListHospitalAppointments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("status is------------ " + response.StatusCode);

            IEnumerable<HospitalAppointmentDto> hospitalAppointments = response.Content.ReadAsAsync<IEnumerable<HospitalAppointmentDto>>().Result;

            return View(hospitalAppointments);
        }


        // GET: HospitalAppointment/Details/5
        public ActionResult Details(int id)
        {
            Debug.WriteLine(" App Controller - Details ");
            // objective : communicate with Appointment api to retrieve one appointment from given id
            //curl:  https://localhost:44325/api/HospitalAppointmentData/

            string url = "HospitalAppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HospitalAppointmentDto SelectedAppointment = response.Content.ReadAsAsync<HospitalAppointmentDto>().Result;

            return View(SelectedAppointment);
        }


        // ----- [U] UPDATE SECTION -----------------------------------------------------------------------------

        // GET:HospitalAppointment/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            string url = "HospitalAppointmentData/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            HospitalAppointmentDto SelectedAppointment = response.Content.ReadAsAsync<HospitalAppointmentDto>().Result;

            return View(SelectedAppointment);
        }

        // Post: HospitalAppointment/Update/5
        [HttpPost]
        public ActionResult Update(int id, HospitalAppointment hospitalAppointment)
        {
            string url = "HospitalAppointmentData/UpdateHospitalAppointment/" + id;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(hospitalAppointment);

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

        // ----- [D] DELETE  SECTION -----------------------------------------------------------------------------

        // GET: HospitalAppointment/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "HospitalAppointment/FindAppointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            HospitalAppointmentDto selectedAppointment = response.Content.ReadAsAsync<HospitalAppointmentDto>().Result;

            return View(selectedAppointment);
        }

        // POST: HospitalAppointment/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            //get token
            GetApplicationCookie();
            string url = "HospitalAppointment/DeleteAppointment/" + id;

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

        public ActionResult Error()
        {
            return View();
        }
    }
}
