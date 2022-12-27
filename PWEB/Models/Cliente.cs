using System;
namespace PWEB.Models
{
	public class Cliente : ApplicationUser
	{
		public ICollection<Reserva> Reservas { get; set; }
    }
}

