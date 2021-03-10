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
    public class PetController : Controller
    {

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        // GET: Pet
        static PetController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44358/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        //Get a list with all the pets
        // GET: Pet/List
        public ActionResult List()
        {
            string url = "petdata/getpets";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<PetDto> SelectedPets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
                return View(SelectedPets);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Details/2
        public ActionResult Details(int id)
        {
            ShowPet ViewModel = new ShowPet();
            string url = "petdata/findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet data transfer object
                PetDto SelectedPet = response.Content.ReadAsAsync<PetDto>().Result;
                ViewModel.Pet = SelectedPet;                

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pet/Create
        [HttpPost]       
        public ActionResult Create(Pet PetInfo)
        {
            Debug.WriteLine(PetInfo.PetName);
            string url = "petdata/addpet";
            Debug.WriteLine(jss.Serialize(PetInfo));
            HttpContent content = new StringContent(jss.Serialize(PetInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Petid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Petid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        // GET: Pet/Edit/5
        public ActionResult Edit(int id)
        {
            UpdatePet ViewModel = new UpdatePet();

            string url = "petdata/findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet data transfer object
                PetDto SelectedPet = response.Content.ReadAsAsync<PetDto>().Result;
                ViewModel.Pet = SelectedPet;

                
                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Pet/Edit/2
        [HttpPost]
        public ActionResult Edit(int id, Pet PetInfo, HttpPostedFileBase PetPic)
        {
            Debug.WriteLine(PetInfo.PetName);
            string url = "petdata/updatepet/" + id;
            Debug.WriteLine(jss.Serialize(PetInfo));
            HttpContent content = new StringContent(jss.Serialize(PetInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                //Send over image data for pet
                url = "pet/updatepetpic/" + id;
                Debug.WriteLine("Received pet picture " + PetPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(PetPic.InputStream);
                requestcontent.Add(imagecontent, "PetPic", PetPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Pet/DeleteConfirm/2
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "petdata/findpet/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into pet data transfer object
                PetDto SelectedPet = response.Content.ReadAsAsync<PetDto>().Result;
                return View(SelectedPet);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Delete Pet
        // POST: Pet/Delete/2
        [HttpPost]
        
        public ActionResult Delete(int id)
        {
            string url = "petdata/deletepet/" + id;
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
