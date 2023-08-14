using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OTTMyPlatform;

public partial class OttplatformContext : DbContext
{
    public OttplatformContext()
    {
    }

    public OttplatformContext(DbContextOptions<OttplatformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserLoginDetail> UserLoginDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=RPN;Initial Catalog=OTTPlatform; TrustServerCertificate=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLoginDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLogi__3214EC07A9EA6005");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
