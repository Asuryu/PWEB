using System;
namespace PWEB.Models
{
	public class Funcionario : ApplicationUser
	{
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}

