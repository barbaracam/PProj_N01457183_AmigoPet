using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amigopet.Models
{
    public class Pet
    {
        //Genrates a database table for the pet entity.
        [Key]
        public int PetID { get; set; }
        public string PetName { get; set; }
        public string PetBreed { get; set; }
        public string PetTip { get; set; }
        public bool PetHasPic { get; set; }

        //If the pet has an image, record the extension of the image (.png, .gif, .jpg, etc.)
        public string PicExtension { get; set; }

        public ICollection<Appointment> Appointments { get; set; }

        
        public ICollection<PetWalker> PetWalkers { get; set; }
    }
    //This class can be used to transfer information about a pet, vessell of communication
    public class PetDto
    {
        public int PetID { get; set; }
        public string PetName { get; set; }
        public string PetBreed { get; set; }
        public string PetTip { get; set; }
        public bool PetHasPic { get; set; }
        public string PicExtension { get; set; }

        


    }










}