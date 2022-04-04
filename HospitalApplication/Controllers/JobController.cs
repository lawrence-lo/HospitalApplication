using HospitalApplication.Models;
using HospitalApplication.Models.ViewModels;
using System;
using System.Collections.Generic;
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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/api/");
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
        public ActionResult Create(Job job)
        {
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
        public ActionResult Update(int id, Job job)
        {
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
        public ActionResult DeleteConfirm(int id)
        {
            string url = "jobdata/findjob/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            JobDto SelectedJob = response.Content.ReadAsAsync<JobDto>().Result;
            return View(SelectedJob);
        }

        // POST: Job/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
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
