using System;
namespace PWEB.Models
{
	public class Gestor : ApplicationUser
    {
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}

