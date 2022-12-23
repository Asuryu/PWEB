using System;
namespace PWEB.Models
{
	public class Empresa
	{

		public int Id { get; set; }
		public string Nome { get; set; }
		public float Avaliacao { get; set; }
		public bool SubscricaoAtiva { get; set; }

		public ICollection<ApplicationUser> Gestores { get; set; }
		public ICollection<ApplicationUser> Funcionarios { get; set; }
        public ICollection<Veiculo> Veiculos { get; set; }
    }
}

