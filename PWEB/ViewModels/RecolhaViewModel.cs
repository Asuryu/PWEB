using System;
using PWEB.Models;

namespace PWEB.ViewModels
{
	public class RecolhaViewModel
	{
        public int ReservaId { get; set; }
        public int NrKmsVeiculo { get; set; }
        public bool Danos { get; set; }
        public string Observacoes { get; set; }
    }
}

