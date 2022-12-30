using System;
namespace PWEB.Models
{
	public class Fotografia
	{
		public int Id { get; set; }
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public byte[] Data { get; set; }

        public int RecolhaVeiculoId { get; set; }
        public RecolhaVeiculo RecolhaVeiculo { get; set; }
    }
}

