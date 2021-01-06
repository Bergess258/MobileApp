﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ConsoleApp7.Models
{
    public partial class danhsa7rd0h0u5Context : DbContext
    {
        public danhsa7rd0h0u5Context()
        {
        }

        public danhsa7rd0h0u5Context(DbContextOptions<danhsa7rd0h0u5Context> options)
            : base(options)
        {
        }

        public virtual DbSet<ActAttending> ActAttendings { get; set; }
        public virtual DbSet<ActCategory> ActCategories { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Quest> Quests { get; set; }
        public virtual DbSet<QuestTask> QuestTasks { get; set; }
        public virtual DbSet<QuestTaskUser> QuestTaskUsers { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserImg> UserImgs { get; set; }
        public virtual DbSet<UserQuest> UserQuests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Trust Server Certificate=True;SSL Mode=Require;Host=ec2-34-248-165-3.eu-west-1.compute.amazonaws.com;Database=danhsa7rd0h0u5;Port=5432;Username=veycugvkaidffx;Password=a9fc1b7d0b47c4dffae1862492b38216ccb686d47324c5bf55e0de09a2a6c590");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<ActAttending>(entity =>
            {
                entity.ToTable("ActAttending");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActAttendings)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FkActivity");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ActAttendings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FkUser");
            });

            modelBuilder.Entity<ActCategory>(entity =>
            {
                entity.ToTable("ActCategory");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Date).HasColumnType("daterange");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("Fk_Category");
            });

            modelBuilder.Entity<Quest>(entity =>
            {
                entity.ToTable("Quest");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name).HasMaxLength(90);
            });

            modelBuilder.Entity<QuestTask>(entity =>
            {
                entity.ToTable("Quest_Task");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.Quest)
                    .WithMany(p => p.QuestTasks)
                    .HasForeignKey(d => d.QuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Quest");

                entity.HasOne(d => d.Task)
                    .WithMany(p => p.QuestTasks)
                    .HasForeignKey(d => d.TaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Task");
            });

            modelBuilder.Entity<QuestTaskUser>(entity =>
            {
                entity.ToTable("Quest_Task_User");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.QuestTaskId).HasColumnName("Quest_TaskId");

                entity.HasOne(d => d.QuestTask)
                    .WithMany(p => p.QuestTaskUsers)
                    .HasForeignKey(d => d.QuestTaskId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Quest_Task");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.QuestTaskUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_User");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsFixedLength(true);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(24)
                    .IsFixedLength(true);

                entity.Property(e => e.Role).HasColumnType("char");
            });

            modelBuilder.Entity<UserImg>(entity =>
            {
                entity.ToTable("UserImg");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserImgs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user");
            });

            modelBuilder.Entity<UserQuest>(entity =>
            {
                entity.ToTable("User_Quest");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.ComplitionDate).HasColumnType("date");

                entity.HasOne(d => d.Quest)
                    .WithMany(p => p.UserQuests)
                    .HasForeignKey(d => d.QuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_QuestId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserQuests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserId");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
