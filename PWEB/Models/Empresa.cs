using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PWEB.Models
{
    public class Empresa
    {

		public int Id { get; set; }

        [Display(Name = "Nome", Prompt = "Introduza o nome da empresa")]
        public string Nome { get; set; }

        [Display(Name = "Avaliação", Prompt = "Introduza uma avaliação", Description = "Avaliação da empresa")]
        [Range(1, 5)]
        public int Avaliacao { get; set; }

        [Display(Name = "Subscrição ativa?")]
        public bool SubscricaoAtiva { get; set; }

		public ICollection<ApplicationUser> Gestores { get; set; }
		public ICollection<ApplicationUser> Funcionarios { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; }
    }
}

