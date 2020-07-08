using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HistorySiteIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HistorySiteIdentity.Models
{
    public class ForceDBContext: IdentityDbContext
    {
       
        public ForceDBContext()
        {
        }

        public ForceDBContext(DbContextOptions<ForceDBContext> options) : base(options)
        { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=ForceDBContext;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Army>()
        //        .HasOne(p => p.BattleFront)
        //        .WithMany(b => b.Armies).HasForeignKey(p => p.BattleFrontId);
        //}
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Customer>()
        //        .HasMany(c => c.Orders)
        //        .WithOptional(o => o.Customer);
        //}
        //protected override void OnModelCreating(ModelBuilder ModelBuilder)
        //{
        //    ModelBuilder.Entity<BattleFront>().HasMany(c => c.Armies);
        //    //ModelBuilder.Entity<BattleFront>().HasMany<Army>();
        //}
        public DbSet<BattleFront> BattleFronts { get; set; }
        public DbSet<Army> Armies { get; set; }
        public DbSet<Corps> Corpss { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Brigade> Brigades { get; set; }
        public DbSet<Regiment> Regiments { get; set; }
        public DbSet<Battalion> Battalions { get; set; }
        public DbSet<HistorySiteIdentity.Models.Week> Week { get; set; }
        public DbSet<HistorySiteIdentity.Models.Commander> Commander { get; set; }
    }
}
