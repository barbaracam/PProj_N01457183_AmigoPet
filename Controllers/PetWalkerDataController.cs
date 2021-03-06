using System;
using System.Collections.Generic;
using System.Web;
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
    public class PetWalkerDataController : ApiController
    {
        private AmigoPetDataContext db = new AmigoPetDataContext();

        // GET: api/PetWalkerData

        // GET: api/PetWalkerData/GetPetWalkers
        [ResponseType(typeof(IEnumerable<PetWalkerDto>))]
        public IHttpActionResult GetPetWalkers()
        {
            List<PetWalker> PetWalkers = db.PetWalkers.ToList();
            List<PetWalkerDto> PetWalkerDtos = new List<PetWalkerDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var PetWalker in PetWalkers)
            {
                PetWalkerDto NewPetWalker = new PetWalkerDto
                {
                    PetWalkerID = PetWalker.PetWalkerID,
                    PetWalkerName = PetWalker.PetWalkerName,
                    PetWalkerBio = PetWalker.PetWalkerBio
                };
                PetWalkerDtos.Add(NewPetWalker);
            }

            return Ok(PetWalkerDtos);
        }

        // GET: api/PetWalkerData/FindPetWalker/2
        [ResponseType(typeof(PetWalkerDto))]
        [HttpGet]
        public IHttpActionResult FindPetWalker(int id)
        {
            PetWalker PetWalker = db.PetWalkers.Find(id);
            if (PetWalker == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            PetWalkerDto PetWalkerDto = new PetWalkerDto
            {
                PetWalkerID = PetWalker.PetWalkerID,                
                PetWalkerName = PetWalker.PetWalkerName,
                PetWalkerBio = PetWalker.PetWalkerBio,
            };


            //pass along data as 200 status code OK response

            return Ok(PetWalkerDto);
        }

        // PUT: api/PetWalkerData/UpdatePetWalker/2
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePetWalker(int id,[FromBody]PetWalker PetWalker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != PetWalker.PetWalkerID)
            {
                return BadRequest();
            }

            db.Entry(PetWalker).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetWalkerExists(id))
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

        // POST: api/PetWalkerData/AddPetWalker
        [ResponseType(typeof(PetWalker))]
        [HttpPost]
        public IHttpActionResult AddPetWalker([FromBody]PetWalker PetWalker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PetWalkers.Add(PetWalker);
            db.SaveChanges();

            return Ok(PetWalker.PetWalkerID);
        }

        // DELETE: api/PetWalkerData/DeletePetWalker/2
        //[ResponseType(typeof(PetWalker))]????
        public IHttpActionResult DeletePetWalker(int id)
        {
            PetWalker PetWalker = db.PetWalkers.Find(id);
            if (PetWalker == null)
            {
                return NotFound();
            }

            db.PetWalkers.Remove(PetWalker);
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

        private bool PetWalkerExists(int id)
        {
            return db.PetWalkers.Count(e => e.PetWalkerID == id) > 0;
        }
    }
}