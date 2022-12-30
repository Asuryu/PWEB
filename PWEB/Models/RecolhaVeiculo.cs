﻿using System;
namespace PWEB.Models
{
	public class RecolhaVeiculo
	{
        public int Id { get; set; }
        public int NrKmsVeiculos { get; set; }
        public bool Danos { get; set; }
        public string Observacoes { get; set; }

        public int FuncionarioId { get; set; }
        public ApplicationUser Funcionario { get; set; }

        public int? ReservaId { get; set; }
        public Reserva Reserva { get; set; }

        public virtual ICollection<Fotografia> Fotografias { get; set; }

    }
}

