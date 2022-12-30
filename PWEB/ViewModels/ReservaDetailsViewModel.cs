using System;
using PWEB.Models;

namespace PWEB.ViewModels
{
	public class ReservaDetailsViewModel
	{
		public Reserva Reserva { get; set; }
		public ApplicationUser Cliente { get; set; }
        public Veiculo Veiculo { get; set; }
        public ApplicationUser Funcionario { get; set; }
        public RecolhaVeiculo Recolha { get; set; }
        public EntregaVeiculo Entrega { get; set; }
    }
}

