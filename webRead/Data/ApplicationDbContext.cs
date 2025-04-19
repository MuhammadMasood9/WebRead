using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using webRead.Models;

namespace webRead.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<DonationMainHead> DonationMainHeads { get; set; }
        public DbSet<PushNotification> PushNotifications { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskProject> Tasks { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<TaxCertificate> TaxCertificates { get; set; }
        public DbSet<DonationDropdown> DonationDropdowns { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // PushNotification configuration
            modelBuilder.Entity<PushNotification>().ToTable("push_notifications", "dbo");
            modelBuilder.Entity<PushNotification>().HasKey(p => p.NotificationId);

            // Banner configuration
            modelBuilder.Entity<Banner>().ToTable("Banners", "dbo");
            modelBuilder.Entity<Banner>().HasKey(b => b.BannerId);

            // Banner configuration
            modelBuilder.Entity<DonationMainHead>().ToTable("DonationMainHeads", "dbo");
            modelBuilder.Entity<DonationMainHead>().HasKey(b => b.MID); modelBuilder.Entity<DonationMainHead>()
        .Property(b => b.Status)
        .HasColumnType("int");


            // Feedback configuration
            modelBuilder.Entity<Feedback>().ToTable("feedback", "dbo");
            modelBuilder.Entity<Feedback>().HasKey(f => f.FeedbackId);

            // Project configuration
            modelBuilder.Entity<Project>().ToTable("Projects", "dbo");
            modelBuilder.Entity<Project>().HasKey(p => p.ProjectId);
            modelBuilder.Entity<Project>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            // TaskProject configuration
            modelBuilder.Entity<TaskProject>().ToTable("Tasks", "dbo");
            modelBuilder.Entity<TaskProject>().HasKey(t => t.TaskId);
            modelBuilder.Entity<TaskProject>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .HasConstraintName("FK_Tasks_Projects_ProjectID")
                .OnDelete(DeleteBehavior.Cascade); // Changed to Cascade
            modelBuilder.Entity<TaskProject>()
                .Property(t => t.ProjectId)
                .HasColumnName("ProjectID");
            modelBuilder.Entity<TaskProject>()
                .Property(t => t.Progress)
                .HasPrecision(5, 2);

            // ProjectUser configuration
            modelBuilder.Entity<ProjectUser>().ToTable("ProjectUsers", "dbo");
            modelBuilder.Entity<ProjectUser>().HasKey(pu => pu.ProjectUserId);
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Project)
                .WithMany()
                .HasForeignKey(pu => pu.ProjectID)
                .HasConstraintName("FK_ProjectUsers_Projects_ProjectID")
                .OnDelete(DeleteBehavior.Cascade); // Changed to Cascade
            modelBuilder.Entity<ProjectUser>()
                .HasOne(pu => pu.Task)
                .WithMany()
                .HasForeignKey(pu => pu.TaskId)
                .HasConstraintName("FK_ProjectUsers_Tasks_TaskID")
                .OnDelete(DeleteBehavior.Cascade); // Changed to Cascade

            // Donor configuration
            modelBuilder.Entity<Donor>().ToTable("Donor", "dbo");
            modelBuilder.Entity<Donor>().HasKey(d => d.CorrespondenceId);

            // TaxCertificate configuration
            modelBuilder.Entity<TaxCertificate>().ToTable("TaxCertificate", "dbo");
            modelBuilder.Entity<TaxCertificate>().HasKey(tc => tc.Id);

            // DonationDropdown configuration
            modelBuilder.Entity<DonationDropdown>().ToTable("DonationDropdown", "dbo");
            modelBuilder.Entity<DonationDropdown>().HasKey(dd => dd.ID);

            // Tickets configuration
            modelBuilder.Entity<Ticket>().ToTable("Tickets", "dbo");
            modelBuilder.Entity<Ticket>().HasKey(t => t.Id);
            modelBuilder.Entity<Ticket>()
                .Property(t => t.TotalAmount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Crossponding_id)
                .HasColumnName("Crossponding_id");
            modelBuilder.Entity<Ticket>()
                .Property(t => t.DontaionType)
                .HasColumnName("DontaionType");

            // Payment configuration
            modelBuilder.Entity<Payment>().ToTable("Payments", "dbo");
            modelBuilder.Entity<Payment>().HasKey(p => p.Id);
            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Payment>()
                .Property(p => p.TotalAmountPaid)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Payment>()
                .Property(p => p.Status)
                .HasColumnType("int")
                .IsRequired(false);
            modelBuilder.Entity<Payment>()
                .Property(p => p.UserCorrespondenceId)
                .HasColumnName("UserCorrespondenceId");
        }
    }
}