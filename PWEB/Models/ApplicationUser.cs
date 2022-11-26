using System;
using Microsoft.AspNetCore.Identity;

namespace PWEB.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PrimeiroNome { get; set; }
        public string UltimoNome { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}

