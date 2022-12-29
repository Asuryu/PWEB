using PWEB_AulasP_2223.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_AulasP_2223.ViewModels
{
    public class GestoresViewModel
    {
        public string UserId { get; set; }
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public string Email { get; set; }
        public string NomeEmpresa { get; set; }
    }
}

