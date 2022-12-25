using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PWEB_AulasP_2223.Models;

namespace PWEB.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        public string Nome { get; set; }
        public int Avaliacao { get; set; }
        public bool SubscricaoAtiva { get; set; }

    }
}
