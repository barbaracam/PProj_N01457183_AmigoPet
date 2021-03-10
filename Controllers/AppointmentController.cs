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
    public class AppointmentController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static AppointmentController()
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


        //Get a list with all the appointments
        // GET: Appointment/List
        public ActionResult List()
        {
            string url = "appointmentdata/getappointments";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<AppointmentDto> SelectedAppointments = response.Content.ReadAsAsync<IEnumerable<AppointmentDto>>().Result;
                return View(SelectedAppointments);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Details/5
        public ActionResult Details(int id)
        {
            ShowAppointment ViewModel = new ShowAppointment();
            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Appointment data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                ViewModel.Appointment = SelectedAppointment;

                //get the pet from the appointment
                url = "Appointmentdata/getpetforappointment/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<PetDto> SelectedPets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
                ViewModel.Pets = SelectedPets;

                //get the pet walker for the appointment
                url = "teamdata/getpetwalkerforappointment/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                //Put data into appointment data transfer object
                IEnumerable<PetWalkerDto> SelectedPetWalkers = response.Content.ReadAsAsync<IEnumerable<PetWalkerDto>>().Result;
                ViewModel.PetWalkers = SelectedPetWalkers;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Create
        public ActionResult Create()
        {
            return View();
        }
        //create appointment
        // POST: Appointment/Create
        [HttpPost]
        public ActionResult Create(Appointment AppointmentInfo)
        {
            Debug.WriteLine(AppointmentInfo.AppointmentTime);
            string url = "appointmentdata/addappointment";
            Debug.WriteLine(jss.Serialize(AppointmentInfo));
            HttpContent content = new StringContent(jss.Serialize(AppointmentInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Appointmentid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Appointmentid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Appointment/Edit/2
        public ActionResult Edit(int id)
        {
            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into appointment data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                return View(SelectedAppointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Appointment/Edit/2
        [HttpPost]
        public ActionResult Edit(int id, Appointment AppointmentInfo)
        {
            Debug.WriteLine(AppointmentInfo.AppointmentTime);
            string url = "Appointmentdata/updateAppointment/" + id;
            Debug.WriteLine(jss.Serialize(AppointmentInfo));
            HttpContent content = new StringContent(jss.Serialize(AppointmentInfo));
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

        [HttpGet]
        // GET: Appointment/DeleteConfirm/2
        
        public ActionResult DeleteConfirm(int id)
        {
            string url = "appointmentdata/findappointment/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into appointment data transfer object
                AppointmentDto SelectedAppointment = response.Content.ReadAsAsync<AppointmentDto>().Result;
                return View(SelectedAppointment);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        //Delete Appointment
        // POST: Appointment/Delete/2
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "appointmentdata/deleteappointment/" + id;
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
