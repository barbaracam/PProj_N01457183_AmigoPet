using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class ShowPetWalker
    {
       public PetWalkerDto PetWalker { get; set; }
       public AppointmentDto Appointment { get; set; }
    }
}