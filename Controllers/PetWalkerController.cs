using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using amigopet.Models;
using amigopet.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;

namespace amigopet.Controllers
{

    public class PetWalkerController : Controller
    {
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        // GET: PetWalker
        static PetWalkerController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44358/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }


        // GET: Pet/List
        public ActionResult List()
        {
            string url = "petwalkerdata/getpetwalkers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PetWalkerDto> SelectedPetWalkers = response.Content.ReadAsAsync<IEnumerable<PetWalkerDto>>().Result;
                return View(SelectedPetWalkers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: PetWalker/Details/5
        public ActionResult Details(int id)
        {
            ShowPetWalker ViewModel = new ShowPetWalker();
            string url = "petwalkerdata/findpetwalker/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into petwalker data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                ViewModel.PetWalker = SelectedPetWalker;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: PetWalker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PetWalker/Create
        [HttpPost]
        public ActionResult Create(PetWalker PetWalkerInfo)
        {
            Debug.WriteLine(PetWalkerInfo.PetWalkerName);
            string url = "petwalkerdata/addpetwalker";
            Debug.WriteLine(jss.Serialize(PetWalkerInfo));
            HttpContent content = new StringContent(jss.Serialize(PetWalkerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int PetWalkerid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = PetWalkerid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: PetWalker/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePetWalker ViewModel = new UpdatePetWalker();

            string url = "petwalkerdata/findpetwalker/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet walker data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                ViewModel.PetWalker = SelectedPetWalker;


                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: PetWalker/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, PetWalker PetWalkerInfo)
        {
            Debug.WriteLine(PetWalkerInfo.PetWalkerName);
            string url = "petwalkerdata/updatepetwalker/" + id;
            Debug.WriteLine(jss.Serialize(PetWalkerInfo));
            HttpContent content = new StringContent(jss.Serialize(PetWalkerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: PetWalker/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "petwalkerdata/findpetwalker/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                PetWalkerDto SelectedPetWalker = response.Content.ReadAsAsync<PetWalkerDto>().Result;
                return View(SelectedPetWalker);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: PetWalker/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "petwalkerdata/deletepetwalker/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
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
