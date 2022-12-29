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
		public Cliente Cliente { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo Veiculo { get; set; }
    }
}

