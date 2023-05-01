using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Labo13.Models;

namespace Labo13.Data
{
    public partial class Labo13Context : DbContext
    {
        public Labo13Context()
        {
        }

        public Labo13Context(DbContextOptions<Labo13Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Changelog> Changelogs { get; set; } = null!;
        public virtual DbSet<Golfeur> Golfeurs { get; set; } = null!;
        public virtual DbSet<ScoreTrou> ScoreTrous { get; set; } = null!;
        public virtual DbSet<VwDetailsScoreGolfeur> VwDetailsScoreGolfeurs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=Labo13");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Changelog>(entity =>
            {
                entity.Property(e => e.InstalledOn).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<ScoreTrou>(entity =>
            {
                entity.HasOne(d => d.Golfeur)
                    .WithMany(p => p.ScoreTrous)
                    .HasForeignKey(d => d.GolfeurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ScorePartie_GolfeurID");
            });

            modelBuilder.Entity<VwDetailsScoreGolfeur>(entity =>
            {
                entity.ToView("VW_DetailsScoreGolfeur", "Golf");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
