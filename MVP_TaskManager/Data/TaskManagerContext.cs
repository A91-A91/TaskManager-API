using System;
using System.Collections.Generic;
using MVP_TaskManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVP_TaskManager.Data;

public partial class TaskManagerContext : DbContext
{

    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StatusRef> StatusRefs { get; set; }

    public virtual DbSet<Models.Task> Tasks { get; set; } // для работы 

    public virtual DbSet<User> Users { get; set; }
   
   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<UserRole>();
        base.OnModelCreating(modelBuilder);
  
        modelBuilder.Entity<StatusRef>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("status_ref_pkey");

            entity.ToTable("status_ref");

            entity.Property(e => e.IdStatus)
                .ValueGeneratedNever()
                .HasColumnName("id_status");
            entity.Property(e => e.NameStatus).HasColumnName("name_status");
        });

        modelBuilder.Entity<Models.Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("tasks_pkey");

            entity.ToTable("tasks");

            entity.Property(e => e.IdTask).ValueGeneratedOnAdd().HasColumnName("id_task");
            entity.Property(e => e.DateCreate).HasColumnName("date_create");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdStatus).HasColumnName("id_status");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdStatus)
                .HasConstraintName("fk_id_status");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_id_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Login)
                .HasColumnName("login");

            entity.Property(e => e.Password)
                .HasColumnName("password");

            entity.Property(e => e.RegDate)
                .HasColumnName("reg_date");

            entity.Property(e => e.Username)
                .HasColumnName("username");
            
            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasConversion<int>();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
