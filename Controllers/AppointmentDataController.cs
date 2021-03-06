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

        /// <summary>
        /// Gets a list of players in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input teamid</param>
        /// <returns>A list of players associated with the team</returns>
        /// <example>
        /// GET: api/TeamData/GetPlayersForTeam
        /// </example>
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

        /// <summary>
        /// Finds Appointment in the database 
        /// </summary>
        /// <param name="id">The Appointment id</param>
        /// <returns>Information about the Appointment</returns>
        // <example>
        // GET: api/AppointmentData/FindAppointment/2

        // </example>
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

        /// <summary>
        /// Updates an Appointment in the database given information about the Team.
        /// </summary>
        /// <param name="id">The Appointment id</param>
        /// <example>
        /// POST: api/AppointmentData/UpdateAppointment/2
        /// </example>



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

        /// <summary>
        /// Adds an Appointment to the database.
        /// </summary>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/AppointmentData/AddAppointment
        /// </example>


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

        /// <summary>
        /// Deletes an Appointment in the database
        /// </summary>
        /// <param name="id">The id of the Appointment to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/AppointmentData/DeleteAppointment/2
        /// </example>
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



        /// <summary>
        /// Finds an Appointment in the system. Internal use only.
        /// </summary>
        private bool AppointmentExists(int id)
        {
            return db.Appointments.Count(e => e.AppointmentID == id) > 0;
        }

       




    }
}