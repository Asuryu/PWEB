using System.ComponentModel.DataAnnotations;
using PWEB_AulasP_2223.Models;

namespace PWEB.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Display(Name ="Categoria")]
        public string Nome { get; set; }
        public string Descricao { get; set; }

    }
}