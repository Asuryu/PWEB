using PWEB_AulasP_2223.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB_AulasP_2223.ViewModels
{
    public class NovoGestorFuncViewModel
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string CargoNaEmpresa { get; set; }
    }
}

