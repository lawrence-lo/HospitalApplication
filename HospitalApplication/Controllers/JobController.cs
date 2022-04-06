using HospitalApplication.Models;
using HospitalApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HospitalApplication.Controllers
{
    public class JobController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static JobController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
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

        // GET: Job/List
        public ActionResult List()
        {
            string url = "jobdata/listjobs";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<JobDto> jobs = response.Content.ReadAsAsync<IEnumerable<JobDto>>().Result;

            return View(jobs);
        }

        // GET: Job/Details/5
        public ActionResult Details(int id)
        {
            string url = "jobdata/findjob/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            JobDto selectedjob = response.Content.ReadAsAsync<JobDto>().Result;

            return View(selectedjob);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Job/New
        [Authorize]
        public ActionResult New()
        {
            //information about all departments
            //Get api/departmentdata/listdepartments

            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;
            
            return View(DepartmentOptions);
        }

        // POST: Job/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Job job)
        {
            GetApplicationCookie();
            string url = "jobdata/addjob";
            string jsonpayload = jss.Serialize(job);

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

        // GET: Job/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateJob ViewModel = new UpdateJob();
            
            //the existing job information
            string url = "jobdata/findjob/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
            ViewModel.SelectedJob = SelectedJob;

            //include all departments to choose from when updating this job
            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentOptions = DepartmentOptions;

            return View(ViewModel);
        }

        // POST: Job/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Job job)
        {
            GetApplicationCookie();
            string url = "jobdata/updatejob/" + id;
            string jsonpayload = jss.Serialize(job);
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

        // GET: Job/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "jobdata/findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
            return View(SelectedJob);
        }

        // POST: Job/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "jobdata/deletejob/" + id;
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
