using System;
namespace PWEB.Models
{
	public class Veiculo
	{
		public int Id { get; set; }
		public string Marca { get; set; }
		public string Modelo { get; set; }
        public string Localizacao { get; set; }
		public string Estado { get; set; }
		public int Custo { get; set; }

        public int CategoriaId { get; set; }
		public Categoria Categoria { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}

