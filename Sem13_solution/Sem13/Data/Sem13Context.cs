using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Sem13.Models;

namespace Sem13.Data
{
    public partial class Sem13Context : DbContext
    {
        public Sem13Context()
        {
        }

        public Sem13Context(DbContextOptions<Sem13Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Changelog> Changelogs { get; set; } = null!;
        public virtual DbSet<Produit> Produits { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Sem13");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.Property(e => e.InstalledOn).HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
