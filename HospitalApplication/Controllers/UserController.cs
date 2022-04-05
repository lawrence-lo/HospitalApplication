using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using HospitalApplication.Models;
using HospitalApplication.Models.ViewModels;
using System.Web.Script.Serialization;

namespace HospitalApplication.Controllers
{
    public class UserController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static UserController()
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

        // GET: User/List
        [Authorize]
        public ActionResult List()
        {
            GetApplicationCookie();//get token credentials
            //objective: communicate with our user data api to retrieve a list of posts
            //curl https://localhost:44325/api/userdata/listusers

            string url = "userdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;


            IEnumerable<UserDto> users = response.Content.ReadAsAsync<IEnumerable<UserDto>>().Result;

            return View(users);
        }

        // GET: User/Details/5
        [Authorize]
        public ActionResult Details(string id)
        {
            GetApplicationCookie();//get token credentials

            DetailsUser ViewModel = new DetailsUser();

            //objective: communicate with our user data api to retrieve one user
            //curl https://localhost:44325/api/userdata/finduser/{id}

            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;

            ViewModel.SelectedUser = SelectedUser;

            //get list of departments
            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentsOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentsOptions = DepartmentsOptions;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        /* User is created from account registration
        // GET: Post/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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
        */

        // GET: User/Edit/5
        [Authorize]
        public ActionResult Edit(string id)
        {
            UpdateUser ViewModel = new UpdateUser();

            //the existing user information
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto SelectedUser = response.Content.ReadAsAsync<UserDto>().Result;
            ViewModel.SelectedUser = SelectedUser;

            //get list of departments
            url = "departmentdata/listdepartments/";
            response = client.GetAsync(url).Result;
            IEnumerable<DepartmentDto> DepartmentsOptions = response.Content.ReadAsAsync<IEnumerable<DepartmentDto>>().Result;

            ViewModel.DepartmentsOptions = DepartmentsOptions;

            return View(ViewModel);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(string id, ApplicationUser user)
        {
            GetApplicationCookie();//get token credentials
            string url = "userdata/updateuser/" + id;
            string jsonpayload = jss.Serialize(user);
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

        // GET: User/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(string id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            UserDto selecteduser = response.Content.ReadAsAsync<UserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(string id)
        {
            GetApplicationCookie();//get token credentials
            string url = "userdata/deleteuser/" + id;
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
