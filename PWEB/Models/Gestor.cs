﻿using System;
namespace PWEB.Models
{
	public class Gestor
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}

