using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amigopet.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        public string AppointmentTime { get; set; }

        public string AppointmentComment { get; set; }


        //Utilizes the inverse property to specify the "Many"
        //https://www.entityframeworktutorial.net/code-first/inverseproperty-dataannotations-attribute-in-code-first.aspx

        

        //One Appointment one Petwalker
        [ForeignKey("PetWalker")]
        public int PetWalkerID { get; set; }
        public virtual PetWalker PetWalker { get; set; }


        //One Appointment one Pet
        [ForeignKey("Pet")]
        public int PetID { get; set; }
        public virtual Pet Pet { get; set; }

    }

    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentComment { get; set; }

        public int PetID { get; set; }

        public int PetWalkerID { get; set; }

        
    }

}