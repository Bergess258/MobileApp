using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace DBWebApi.Models
{
    public partial class DBContx : DbContext
    {
        public DBContx()
        {
        }

        public DBContx(DbContextOptions<DBContx> options)
            : base(options)
        {
        }

        public virtual DbSet<ActAttending> ActAttending { get; set; }
        public virtual DbSet<ActCategory> ActCategories { get; set; }
        public virtual DbSet<ActChat> ActChats { get; set; }
        public virtual DbSet<Activity> Activities { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Quest> Quests { get; set; }
        public virtual DbSet<QuestTask> QuestTasks { get; set; }
        public virtual DbSet<QuestTaskUser> QuestTaskUser { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserImg> UserImg { get; set; }
        public virtual DbSet<UserQuest> UserQuests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
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

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActCategories)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ActivityId");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.ActCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CategoryId");
            });

            modelBuilder.Entity<ActChat>(entity =>
            {
                entity.ToTable("ActChat");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasColumnType("character varying");

                entity.HasOne(d => d.Activity)
                    .WithMany(p => p.ActChat)
                    .HasForeignKey(d => d.ActivityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_ActivityId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ActChat)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_UserId");
            });

            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("Activity");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.EndD).HasColumnType("timestamp with time zone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.StartD).HasColumnType("timestamp with time zone");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(256);
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
                    .WithMany(p => p.QuestTaskUser)
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

                entity.Property(e => e.Role).HasMaxLength(1);
            });

            modelBuilder.Entity<UserImg>(entity =>
            {
                entity.ToTable("UserImg");

                entity.Property(e => e.Id).UseIdentityAlwaysColumn();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserImg)
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
