using HospitalApplication.Models;
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
            return View();
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
            return View();
        }

        // POST: Job/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Job/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Job/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
