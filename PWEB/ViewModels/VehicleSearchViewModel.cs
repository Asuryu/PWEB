﻿using PWEB.Models;
using PWEB_AulasP_2223.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_AulasP_2223.ViewModels
{
    public class VehicleSearchViewModel
    {
        public string? Location { get; set; }
        public string? Category { get; set; }
        public DateTime PickupDateAndTime { get; set; }
        public DateTime ReturnDateAndTime { get; set; }
        public List<Veiculo> Veiculos { get; set; }
    }
}
