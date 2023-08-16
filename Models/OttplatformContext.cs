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

    public virtual DbSet<Role> Roles { get; set; }

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
            entity.HasKey(e => e.EpisodeId).HasName("PK__Episode__AC667615E62847B9");

            entity.ToTable("Episode");

            entity.Property(e => e.EpisodeId).HasColumnName("EpisodeID");
            entity.Property(e => e.ShowId).HasColumnName("ShowID");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AD1BE6D8F");

            entity.Property(e => e.RoleType)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tvshow>(entity =>
        {
            entity.HasKey(e => e.ShowId).HasName("PK__TVShow__6DE3E0D23525B02C");

            entity.ToTable("TVShow");

            entity.Property(e => e.ShowId).HasColumnName("ShowID");
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
            entity.HasKey(e => e.Id).HasName("PK__UserLogi__3214EC070AF0EC92");

            entity.ToTable("UserLoginDetail");

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
            entity.HasKey(e => e.UserWatchListId).HasName("PK__UserShow__8FDB9D7757A1C290");

            entity.ToTable("UserShowWatchList");

            entity.Property(e => e.UserWatchListId).HasColumnName("UserWatchListID");
            entity.Property(e => e.EpisodeId).HasColumnName("EpisodeID");
            entity.Property(e => e.ShowId).HasColumnName("ShowID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
