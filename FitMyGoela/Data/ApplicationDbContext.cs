using FitMyGoela.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitMyGoela.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TiposDeTreino> TiposDeTreinos { get; set; }
    }
}
