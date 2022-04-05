﻿using HospitalApplication.Models;
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
    public class DepartmentController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DepartmentController()
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

        // GET: Department/List
        public ActionResult List()
        {
            string url = "departmentdata/listdepartments";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DepartmentDto> departments = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            return View(departments);
        }

        // GET: Department/Details/5
        public ActionResult Details(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            DepartmentDto selecteddepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;

            /* To-do */
            //show employees related to this department

            return View(selecteddepartment);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Department/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Department department)
        {
            GetApplicationCookie();
            string url = "departmentdata/adddepartment";
            string jsonpayload = jss.Serialize(department);

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

        // GET: Department/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdateDepartment ViewModel = new UpdateDepartment();
            
            //existing department information
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            ViewModel.SelectedDepartment = SelectedDepartment;

            /* To-do */
            //include all employees to choose from when updating this department

            return View(ViewModel);
        }

        // POST: Department/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Department department)
        {
            GetApplicationCookie();
            string url = "departmentdata/updatedepartment/" + id;
            string jsonpayload = jss.Serialize(department);
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

        // GET: Department/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "departmentdata/finddepartment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DepartmentDto SelectedDepartment = response.Content.ReadAsAsync<DepartmentDto>().Result;
            return View(SelectedDepartment);
        }

        // POST: Department/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();
            string url = "departmentdata/deletedepartment/" + id;
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
