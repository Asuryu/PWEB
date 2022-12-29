using System;
using Microsoft.AspNetCore.Identity;
using PWEB_AulasP_2223.Models;

namespace PWEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }

        public ICollection<Reserva> Reservas { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public string? CargoNaEmpresa { get; set; }
    }
}