using Microsoft.EntityFrameworkCore;
using Vega.Models;

namespace Vega.Persistence
{
    public class VegaDbContext : DbContext
    {
        public VegaDbContext(DbContextOptions<VegaDbContext> options) : base(options)
        {

        }

        public DbSet<Make> Makes {get; set;} // It is not necessary to add Model class to DbSet because Make class has reference to Collection of Model class so EF will recognise Model class as part of domain model
        public DbSet<Feature> Features {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>().HasKey(vf =>
            new {vf.VehicleId, vf.FeatureId});
        }
    }
}