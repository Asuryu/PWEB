using System;
namespace PWEB.Models
{
	public class EntregaVeiculo
	{
		public int Id { get; set; }
		public int NrKmsVeiculos { get; set; }
		public bool Danos { get; set; }
		public string Observacoes { get; set; }

		public int FuncionarioId { get; set; }
		public ApplicationUser Funcionario { get; set; }

    }
}

