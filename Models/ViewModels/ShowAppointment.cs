using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace amigopet.Models.ViewModels
{
    public class ShowAppointment
    {
        //Information about the Appointment
        public AppointmentDto Appointment { get; set; }

        //Information about all Pets on that Appointment
        public IEnumerable<PetDto> Pets { get; set; }

        //Information about all PetWalker for the Appointment
        public IEnumerable<PetWalkerDto> PetWalkers { get; set; }

    }
}