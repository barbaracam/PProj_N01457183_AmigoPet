using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using amigopet.Models;
using System.Diagnostics;
using System.IO;

namespace amigopet.Controllers
{
    public class AppointmentDataController : ApiController
    {
        //database access point
        private AmigoPetDataContext db = new AmigoPetDataContext();


        /// <summary>
        /// Gets a list of Appointment in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of Appointments</returns>
        

        // GET: api/AppointmentData/GetAppointments
        [ResponseType(typeof(IEnumerable<AppointmentDto>))]
        public IHttpActionResult GetAppointments()
        {
            List<Appointment> Appointments = db.Appointments.ToList();
            List<AppointmentDto> AppointmentDtos = new List<AppointmentDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Appointment in Appointments)
            {
                AppointmentDto NewAppointment = new AppointmentDto
                {
                    AppointmentID = Appointment.AppointmentID,
                    AppointmentTime = Appointment.AppointmentTime,
                    AppointmentComment = Appointment.AppointmentComment,
                    PetWalkerID = Appointment.PetWalkerID,
                    PetID = Appointment.PetID
                    
                };
                AppointmentDtos.Add(NewAppointment);
            }

            return Ok(AppointmentDtos);
        }

        
        /// Gets a list of pet for the appointment     
        /// GET: api/appointmentData/GetPetForAppointment
        
        [HttpGet]
        [ResponseType(typeof(PetDto))]
        public IHttpActionResult GetPetForAppointment(int id)
        {
            Pet Pet = db.Pets
                .Where(p => p.Appointments.Any(a => a.AppointmentID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Pet == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            PetDto PetDto = new PetDto
            {
                PetID = Pet.PetID,
                PetName = Pet.PetName,
                PetBreed = Pet.PetBreed,
            };

            //pass along data as 200 status code OK response
            return Ok(PetDto);
        }

        /// Gets a list of petwalker for the appointment     
        /// GET: api/appointmentData/GetPetWalkerForAppointment
        [HttpGet]
            [ResponseType(typeof(PetWalkerDto))]
            public IHttpActionResult GetPetWalkerForAppointment(int id)
            {
                PetWalker PetWalker = db.PetWalkers
                    .Where(pw => pw.Appointments.Any(a => a.AppointmentID == id))
                    .FirstOrDefault();
                //if not found, return 404 status code.
                if (PetWalker == null)
                {
                    return NotFound();
                }

                //put into a 'friendly object format'
                PetWalkerDto PetWalkerDto = new PetWalkerDto
                {
                    PetWalkerID = PetWalker.PetWalkerID,
                    PetWalkerName = PetWalker.PetWalkerName,
                    PetWalkerBio = PetWalker.PetWalkerBio
                };


                //pass along data as 200 status code OK response
                return Ok(PetWalkerDto);
            }

        //Finds a pet in the database with a 200 status code.
        // GET: api/AppointmentData/FindAppointment/2

        [ResponseType(typeof(AppointmentDto))]
        [HttpGet]
        public IHttpActionResult FindAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            if (Appointment == null)
            {
                return NotFound();
            }

            //put into a 'object format'
            AppointmentDto AppointmentDto = new AppointmentDto
            {
                AppointmentID = Appointment.AppointmentID,
                AppointmentTime = Appointment.AppointmentTime,
                AppointmentComment = Appointment.AppointmentComment,
                PetWalkerID = Appointment.PetWalkerID,
                PetID = Appointment.PetID
            };

            //pass along data as 200 status code OK response

            return Ok(AppointmentDto);
        }

        
        /// Updates an Appointment in the database
        // PUT: api/AppointmentData/UpdateAppointment/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointment(int id, [FromBody]Appointment Appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Appointment.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(Appointment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        
        /// Adds an Appointment to the database.
        // POST: api/AppointmentData/AddAppointment

        [ResponseType(typeof(Appointment))]
        [HttpPost]
        public IHttpActionResult AddAppointment([FromBody]Appointment Appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Appointments.Add(Appointment);
            db.SaveChanges();

            return Ok(Appointment.AppointmentID);
        }

        
        /// Deletes an Appointment in the database
        /// POST: api/AppointmentData/DeleteAppointment/2
       
        public IHttpActionResult DeleteAppointment(int id)
        {
            Appointment Appointment = db.Appointments.Find(id);
            if (Appointment == null)
            {
                return NotFound();
            }

            db.Appointments.Remove(Appointment);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// Finds an Appointment in the system. Internal use only.
       
        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }

       
     }
}