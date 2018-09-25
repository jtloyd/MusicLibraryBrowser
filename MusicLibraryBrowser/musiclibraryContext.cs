using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MusicLibraryBrowser
{
    public partial class musiclibraryContext : DbContext
    {
        public musiclibraryContext()
        {
        }

        public musiclibraryContext(DbContextOptions<musiclibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artist { get; set; }
        public virtual DbSet<Genre> Genre { get; set; }
        public virtual DbSet<Work> Work { get; set; }
        public virtual DbSet<WorkVersion> WorkVersion { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=musiclibrary;Username=postgres;Password=postgres");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Artist>(entity =>
            {
                entity.ToTable("artist");

                entity.Property(e => e.ArtistId)
                    .HasColumnName("artist_id")
                    .UseNpgsqlIdentityAlwaysColumn();

                entity.Property(e => e.ArtistName)
                    .IsRequired()
                    .HasColumnName("artist_name")
                    .HasColumnType("character varying");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Artist)
                    .HasForeignKey(d => d.GenreId)
                    .HasConstraintName("artist_genre_id_fkey");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genre");

                entity.Property(e => e.GenreId)
                    .HasColumnName("genre_id")
                    .UseNpgsqlIdentityAlwaysColumn();

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasColumnName("genre_name")
                    .HasColumnType("character varying");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("work");

                entity.Property(e => e.WorkId)
                    .HasColumnName("work_id")
                    .UseNpgsqlIdentityAlwaysColumn();

                entity.Property(e => e.ArtistId).HasColumnName("artist_id");

                entity.Property(e => e.WorkName)
                    .IsRequired()
                    .HasColumnName("work_name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Work)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("work_artist_id_fkey");
            });

            modelBuilder.Entity<WorkVersion>(entity =>
            {
                entity.ToTable("work_version");

                entity.Property(e => e.WorkVersionId)
                    .HasColumnName("work_version_id")
                    .UseNpgsqlIdentityAlwaysColumn();

                entity.Property(e => e.Lossless).HasColumnName("lossless");

                entity.Property(e => e.WorkId).HasColumnName("work_id");

                entity.Property(e => e.WorkVersionName)
                    .IsRequired()
                    .HasColumnName("work_version_name")
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Work)
                    .WithMany(p => p.WorkVersion)
                    .HasForeignKey(d => d.WorkId)
                    .HasConstraintName("work_version_work_id_fkey");
            });
        }
    }
}
