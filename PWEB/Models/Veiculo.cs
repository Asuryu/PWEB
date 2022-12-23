using System;
namespace PWEB.Models
{
	public class Veiculo
	{

		public int Id { get; set; }
		public String? Tipo { get; set; }
		public String? Localizacao { get; set; }
		public String? Estado { get; set; }
		public String? Categoria { get; set; }
		public float Custo { get; set; }

		public int? EmpresaId { get; set; }
		public Empresa Empresa { get; set; }

	}
}

