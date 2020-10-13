using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace TaskManagementDomain.Models
{
    public partial class TaskManagementDatabaseSystemContext : DbContext
    {
        public TaskManagementDatabaseSystemContext()
        {
        }

        public TaskManagementDatabaseSystemContext(DbContextOptions<TaskManagementDatabaseSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<SubTask> SubTask { get; set; }
        public virtual DbSet<Task> Task { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=LAPTOP-KTVD108G\\SQLEXPRESS;Initial Catalog=TaskManagementDatabaseSystem;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubTask>(entity =>
            {
                entity.Property(e => e.SubTaskId).HasColumnName("SubTaskID");

                entity.Property(e => e.FinishDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.SubTask)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__SubTask__TaskID__38996AB5");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.Property(e => e.TaskId).HasColumnName("TaskID");

                entity.Property(e => e.FinishDate).HasColumnType("date");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.TaskDescription)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
