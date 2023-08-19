using Caritas.Common.Models;
using Caritas.Insfrastructure.Model;
using Caritas.Insfrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Caritas.Insfrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
               : base(options)
        {
        }

        public DbSet<Adjudicado> Adjudicados { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Dest> Dests { get; set; }
        public DbSet<EstadoC> EstadoCs { get; set; }
        public DbSet<EstadoI> EstadoIs { get; set; }
        public DbSet<EstadoU> EstadoUs { get; set; }
        public DbSet<Inhumado> Inhumados { get; set; }
        public DbSet<TipoUbicacion> TipoUbicacions { get; set; }
        public DbSet<Ubicacion> Ubicacions { get; set; }
        public DbSet<Calendario>  Calendario { get; set; }
        public DbSet<PlantillaEmail> PlantillasEmail { get; set; }
        public DbSet<Aviso> Avisos { get; set; }
        public DbSet<Resultado> Resultados { get; set; }

    }
}
