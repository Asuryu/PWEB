using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PWEB.Models;
using PWEB_AulasP_2223.Models;
using System.Reflection.Metadata;

namespace PWEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<ApplicationUser> Administradores { get; set; }
        public DbSet<ApplicationUser> Gestores { get; set; }
        public DbSet<ApplicationUser> Funcionarios { get; set; }
        public DbSet<ApplicationUser> Clientes { get; set; }
        public DbSet<ApplicationUser> Utilizadores { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}