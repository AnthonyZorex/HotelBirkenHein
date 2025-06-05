using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Motel_birkenhein.Models;
using System.Net.NetworkInformation;

namespace Motel_birkenhein.Data
{
    public class MotelBirkenheinData : IdentityDbContext<User>
    {
        public MotelBirkenheinData(DbContextOptions<MotelBirkenheinData> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var adminRole = new IdentityRole
            {
                Id = "6dc37c9b-3265-47c0-8e57-0ce1b7b85d92",
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var mitarbeiterRole = new IdentityRole
            {
                Id = "53c712d3-9da5-4da1-881f-9a9db868645c",
                Name = "mitarbeiter",
                NormalizedName = "MITARBEITER"
            };

            builder.Entity<Tarif>()
           .HasOne(z => z.XHotel)
           .WithMany(h => h.Tarif)
           .HasForeignKey(z => z.XHotelId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Zimmer>()
            .HasOne(z => z.XHotels)
            .WithMany(h => h.Zimmer)
            .HasForeignKey(z => z.XHotelId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bett>()
              .HasOne(z => z.XZimmer)
              .WithMany(h => h.Bett)
              .HasForeignKey(z => z.XZimmerId)
              .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Gast>()
             .HasOne(z => z.XReservierung)
             .WithMany(h => h.Gast)
             .HasForeignKey(z => z.XReservierungId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Firma>()
             .HasOne(z => z.XReservierung)
             .WithMany(h => h.Firma)
             .HasForeignKey(z => z.XReservierungId)
             .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Zimmer>()
          .HasOne(z => z.XReservierung)
          .WithMany(h => h.Zimmer)
          .HasForeignKey(z => z.XReservierungId)
          .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bett>()
              .HasOne(z => z.XReservierung)
              .WithMany(h => h.Bett)
              .HasForeignKey(z => z.XReservierungId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<IdentityRole>().HasData(adminRole, mitarbeiterRole);
        }
     
        public DbSet<User> User { get; set; }
        public DbSet<Hotel> Hotel { get; set; }
        public DbSet<Tarif> Tarif { get; set; }
        public DbSet<Zimmer> Zimmer { get; set; }
        public DbSet<Bett> Bett  { get; set; }
        public DbSet<Reservierung> Reservierung { get; set; }
        public DbSet<Gast> Gast { get; set; }
        public DbSet<Firma> Firma { get; set; }
    }
}
