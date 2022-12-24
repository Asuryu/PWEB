using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PWEB.Models;

namespace PWEB.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{

    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Veiculo> Veiculos { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}