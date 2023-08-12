using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OTTMyPlatform.Models;

public partial class OttplatformContext : DbContext
{
    public OttplatformContext()
    {
    }

    public OttplatformContext(DbContextOptions<OttplatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Episode> Episodes { get; set; }

    public virtual DbSet<Tvshow> Tvshows { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLoginDetail> UserLoginDetails { get; set; }

    public virtual DbSet<UserShowWatchList> UserShowWatchLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=RPN;Initial Catalog=OTTPlatform; TrustServerCertificate=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Episode>(entity =>
        {
            entity.HasKey(e => e.EpisodeId).HasName("PK__Episode__AC6676150E040EE1");

            entity.ToTable("Episode");

            entity.Property(e => e.EpisodeId)
                .ValueGeneratedNever()
                .HasColumnName("EpisodeID");
            entity.Property(e => e.ShowId).HasColumnName("ShowID");

            entity.HasOne(d => d.Show).WithMany(p => p.Episodes)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK__Episode__ShowID__286302EC");
        });

        modelBuilder.Entity<Tvshow>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__TVShow__6DE3E0D28F824CB7");

            entity.ToTable("TVShow");

            entity.Property(e => e.ShowId)
                .ValueGeneratedNever()
                .HasColumnName("ShowID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CE0C87151");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserLoginDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserLoginDetail");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserShowWatchList>(entity =>
        {
            entity.HasKey(e => e.UserWatchListId).HasName("PK__UserShow__8FDB9D77410B8F61");

            entity.ToTable("UserShowWatchList");

            entity.Property(e => e.UserWatchListId)
                .ValueGeneratedNever()
                .HasColumnName("UserWatchListID");
            entity.Property(e => e.EpisodeId).HasColumnName("EpisodeID");
            entity.Property(e => e.ShowId).HasColumnName("ShowID");

            entity.HasOne(d => d.Episode).WithMany(p => p.UserShowWatchLists)
                .HasForeignKey(d => d.EpisodeId)
                .HasConstraintName("FK__UserShowW__Episo__33D4B598");

            entity.HasOne(d => d.Show).WithMany(p => p.UserShowWatchLists)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK__UserShowW__ShowI__31EC6D26");

            entity.HasOne(d => d.User).WithMany(p => p.UserShowWatchLists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserShowW__UserI__32E0915F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
