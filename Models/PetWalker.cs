using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amigopet.Models
{
    public class PetWalker
    {

        [Key]
        public int PetWalkerID { get; set; }

        public string PetWalkerName { get; set; }

        public string PetWalkerBio { get; set; }


        //Utilizes the inverse property to specify the "Many"
        //https://www.entityframeworktutorial.net/code-first/inverseproperty-dataannotations-attribute-in-code-first.aspx

        //One PetWalker can have many appointments
        public ICollection<Appointment> Appointments { get; set; }

              
        public ICollection<PetWalker> PetWalkers { get; set; }





    }

    public class PetWalkerDto
    {
        public int PetWalkerID { get; set; }
        public string PetWalkerName { get; set; }
        public string PetWalkerBio { get; set; }


    }

}