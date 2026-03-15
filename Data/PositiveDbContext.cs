using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PositivePOSAPI.Models;

namespace PositivePOSAPI.Data;

public partial class PositiveDbContext : DbContext
{
    public PositiveDbContext(DbContextOptions<PositiveDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppPage> AppPages { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<UserApproval> UserApprovals { get; set; }

    public virtual DbSet<UserAssignment> UserAssignments { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppPage>(entity =>
        {
            entity.HasKey(e => e.PageGuid).HasName("PK_Admin_AppPages");

            entity.ToTable("AppPages", "Admin");

            entity.HasIndex(e => new { e.IsActive, e.SortOrder }, "IX_Admin_AppPages_IsActive");

            entity.HasIndex(e => e.PageKey, "UQ_Admin_AppPages_PageKey").IsUnique();

            entity.Property(e => e.PageGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DisplayName).HasMaxLength(150);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.PageKey).HasMaxLength(100);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "UX_AspNetRoles_NormalizedName")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_AspNetRoleClaims_Roles_RoleId");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "IX_AspNetUsers_NormalizedEmail");

            entity.HasIndex(e => e.NormalizedUserName, "UX_AspNetUsers_NormalizedUserName")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.DateLastLogin).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NameFirst).HasMaxLength(100);
            entity.Property(e => e.NameLast).HasMaxLength(100);
            entity.Property(e => e.NameMiddle).HasMaxLength(100);
            entity.Property(e => e.NamePrefix).HasMaxLength(50);
            entity.Property(e => e.NameSuffix).HasMaxLength(50);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_AspNetUserRoles_Roles_RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_AspNetUserRoles_Users_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserClaims_Users_UserId");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserLogins_Users_UserId");
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_AspNetUserTokens_Users_UserId");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyGuid).HasName("PK_Admin_Companies");

            entity.ToTable("Companies", "Admin");

            entity.HasIndex(e => e.IsActive, "IX_Admin_Companies_IsActive");

            entity.HasIndex(e => e.CompanyNumber, "UQ__Companie__9E521C9CC8C57773").IsUnique();

            entity.Property(e => e.CompanyGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ContactNameFirst).HasMaxLength(100);
            entity.Property(e => e.ContactNameLast).HasMaxLength(100);
            entity.Property(e => e.CreatedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LegalName).HasMaxLength(250);
            entity.Property(e => e.ModifiedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(30);
            entity.Property(e => e.TaxId).HasMaxLength(50);
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.LocationGuid).HasName("PK_Admin_Locations");

            entity.ToTable("Locations", "Admin");

            entity.HasIndex(e => new { e.CompanyGuid, e.IsActive }, "IX_Admin_Locations_Active");

            entity.HasIndex(e => e.CompanyGuid, "IX_Admin_Locations_CompanyGuid");

            entity.HasIndex(e => new { e.CompanyGuid, e.LocationNumber }, "UQ_Admin_Locations_Company_LocNum").IsUnique();

            entity.Property(e => e.LocationGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Address1).HasMaxLength(200);
            entity.Property(e => e.Address2).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasDefaultValue("US");
            entity.Property(e => e.CreatedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.State).HasMaxLength(50);
            entity.Property(e => e.Timezone)
                .HasMaxLength(80)
                .HasDefaultValue("America/Chicago");

            entity.HasOne(d => d.Company).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CompanyGuid)
                .HasConstraintName("FK_Admin_Locations_Companies");
        });

        modelBuilder.Entity<UserApproval>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_Admin_UserApproval");

            entity.ToTable("UserApproval", "Admin");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.ApprovedUtc).HasPrecision(3);
            entity.Property(e => e.CreatedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.User).WithOne(p => p.UserApproval)
                .HasForeignKey<UserApproval>(d => d.UserId)
                .HasConstraintName("FK_Admin_UserApproval_AspNetUsers");
        });

        modelBuilder.Entity<UserAssignment>(entity =>
        {
            entity.HasKey(e => e.UserAssignmentGuid).HasName("PK_Admin_UserAssignments");

            entity.ToTable("UserAssignments", "Admin");

            entity.Property(e => e.UserAssignmentGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModifiedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.UserPermissionGuid).HasName("PK_Admin_UserPermissions");

            entity.ToTable("UserPermissions", "Admin");

            entity.HasIndex(e => e.UserId, "IX_Admin_UserPermissions_UserId");

            entity.HasIndex(e => new { e.UserId, e.PageGuid }, "UQ_Admin_UserPermissions_User_Page").IsUnique();

            entity.Property(e => e.UserPermissionGuid).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedUtc)
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.ModifiedUtc)
            
                .HasPrecision(3)
                .HasDefaultValueSql("(sysutcdatetime())");

            entity.HasOne(d => d.Page).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PageGuid)
                .HasConstraintName("FK_Admin_UserPermissions_AppPages");

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Admin_UserPermissions_AspNetUsers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
