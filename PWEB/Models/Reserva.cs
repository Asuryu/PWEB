using System;
namespace PWEB.Models
{
	public class Reserva
	{
		public int Id { get; set; }

		public DateTime DataLevantamento { get; set; }
		public DateTime DataEntrega { get; set; }
		public bool Confirmada { get; set; }

		public int ClienteId { get; set; }
		public ApplicationUser Cliente { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }

		public int? RecolhaVeiculoId { get; set; }
        public RecolhaVeiculo RecolhaVeiculo { get; set; }

        public int? EntregaVeiculoId { get; set; }
        public EntregaVeiculo EntregaVeiculo { get; set; }

    }
}

