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
    public class PostController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PostController()
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

        // GET: Post/List
        public ActionResult List()
        {
            //objective: communicate with our post data api to retrieve a list of posts
            //curl https://localhost:44325/api/postdata/listposts

            string url = "postdata/listposts";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PostDto> posts = response.Content.ReadAsAsync<IEnumerable<PostDto>>().Result;

            return View(posts);
        }

        // GET: Post/Details/5
        public ActionResult Details(int id)
        {
            DetailsPost ViewModel = new DetailsPost();

            //objective: communicate with our post data api to retrieve one post
            //curl https://localhost:44325/api/postdata/findpost/{id}

            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PostDto SelectedPost = response.Content.ReadAsAsync<PostDto>().Result;

            ViewModel.SelectedPost = SelectedPost;

            //Todo: put user info in viewmodel

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Post/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [Authorize]
        public ActionResult Create(Post post)
        {
            GetApplicationCookie();//get token credentials
            //objective: add a new post into our system using the API
            //curl -H "Content-Type:application/json" -d @post.json https://localhost:44325/api/postdata/addpost 
            string url = "postdata/addpost";

            string jsonpayload = jss.Serialize(post);

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

        // GET: Post/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            UpdatePost ViewModel = new UpdatePost();

            //the existing post information
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PostDto SelectedPost = response.Content.ReadAsAsync<PostDto>().Result;
            ViewModel.SelectedPost = SelectedPost;

            return View(ViewModel);
        }

        // POST: Post/Update/5
        [HttpPost]
        public ActionResult Update(int id, Post post)
        {
            GetApplicationCookie();//get token credentials
            string url = "postdata/updatepost/" + id;
            string jsonpayload = jss.Serialize(post);
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

        // GET: Post/Delete/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "postdata/findpost/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PostDto selectedpost = response.Content.ReadAsAsync<PostDto>().Result;
            return View(selectedpost);
        }

        // POST: Post/Delete/5
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id)
        {
            GetApplicationCookie();//get token credentials
            string url = "postdata/deletepost/" + id;
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
