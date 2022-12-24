using System;
namespace PWEB.Models
{
	public class Veiculo
	{

		public int Id { get; set; }
		public string Tipo { get; set; }
		public string Localizacao { get; set; }
		public string Estado { get; set; }
		public string Categoria { get; set; }
		public float Custo { get; set; }

		public int? EmpresaId { get; set; }
		public Empresa Empresa { get; set; }

	}
}

