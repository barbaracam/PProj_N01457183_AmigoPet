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
    public class PetDataController : ApiController
    {
        private AmigoPetDataContext db = new AmigoPetDataContext();

        // GET: api/PetData/GetPets
        [ResponseType(typeof(IEnumerable<PetDto>))]
        public IHttpActionResult GetPets()
        {
            List<Pet> Pets = db.Pets.ToList();
            List<PetDto> PetDtos = new List<PetDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Pet in Pets)
            {
                PetDto NewPet = new PetDto
                {
                    PetID = Pet.PetID,
                    PetName = Pet.PetName,
                    PetBreed = Pet.PetBreed,
                    PetTip = Pet.PetTip,
                    PetHasPic = Pet.PetHasPic,
                    PicExtension = Pet.PicExtension,



                };
                PetDtos.Add(NewPet);
            }

            return Ok(PetDtos);
        }
        // GET: api/PetData/FindPet/2
        [HttpGet]
        [ResponseType(typeof(PetDto))]
        public IHttpActionResult FindPet(int id)
        {
            //Find the data
            Pet Pet = db.Pets.Find(id);
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
                PetTip = Pet.PetTip
            };
            //pass along data as 200 status code OK response
            return Ok(PetDto);
        }



        // POST: api/PetData/UpdatePet/2
        // FORM DATA: Pet JSON Object
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePet(int id, [FromBody]Pet Pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Pet.PetID)
            {
                return BadRequest();
            }

            db.Entry(Pet).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(id))
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

        
        [HttpPost]
        public IHttpActionResult UpdatePetPic(int id)
        {

            bool HasPic = false;
            string PicExtension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var PetPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (PetPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(PetPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Pets/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Pets/"), fn);

                                //save the file
                                PetPic.SaveAs(path);

                                //if these are all successful then we can set these fields
                                HasPic = true;
                                PicExtension = extension;

                                //Update the pet haspic and picextension fields in the database
                                Pet SelectedPet = db.Pets.Find(id);
                                SelectedPet.PetHasPic = HasPic;
                                SelectedPet.PicExtension = extension;
                                db.Entry(SelectedPet).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Pet Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }

        // POST: api/PetData/AddPet
        // FORM DATA: Player JSON Object
        [ResponseType(typeof(Pet))]
        [HttpPost]
        public IHttpActionResult AddPet([FromBody]Pet Pet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Pets.Add(Pet);
            db.SaveChanges();

            return Ok(Pet.PetID);
        }

        // DELETE: api/PetData/DeletePet/2
        [HttpPost]
        //[ResponseType(typeof(Pet))]????
        public IHttpActionResult DeletePet(int id)
        {
            Pet Pet = db.Pets.Find(id);
            if (Pet == null)
            {
                return NotFound();
            }

            string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Pets/" + id + "." + Pet.PicExtension);
            if (System.IO.File.Exists(path))
            {
                Debug.WriteLine("File exists... preparing to delete!");
                System.IO.File.Delete(path);
            }

            db.Pets.Remove(Pet);
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

        private bool PetExists(int id)
        {
            return db.Pets.Count(e => e.PetID == id) > 0;
        }
    }
}