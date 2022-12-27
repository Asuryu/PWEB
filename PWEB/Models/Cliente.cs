using System;
namespace PWEB.Models
{
	public class Cliente
	{
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<Reserva> Reservas { get; set; }
		//public ICollection<Reserva> HistoricoReservas { get; set; }
    }
}

