using System;
using System.Collections.Generic;
using Aptitude_Test.Models;
using Microsoft.EntityFrameworkCore;

namespace Aptitude_Test.Data;

public partial class AptitudeTestContext : DbContext
{
    public AptitudeTestContext()
    {
    }

    public AptitudeTestContext(DbContextOptions<AptitudeTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<ComputerTest> ComputerTests { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Finalresult> Finalresults { get; set; }

    public virtual DbSet<Gktest> Gktests { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<JobApplication> JobApplications { get; set; }

    public virtual DbSet<MathTest> MathTests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.,65387;Initial Catalog=Aptitude_Test;Persist Security Info=False;User ID=shehriyar;Password=asdf1234;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539BFC913ACEEC");

            entity.Property(e => e.CandidateId).HasColumnName("CandidateID");
            entity.Property(e => e.CantDescription)
                .HasColumnType("text")
                .HasColumnName("Cant_Description");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ComputerTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Computer__3214EC275AF5ED61");

            entity.ToTable("ComputerTest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.OptionA)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionB)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionC)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionD)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Question).IsUnicode(false);
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.ContId).HasName("PK__CONTACT__33DC4C79006691E5");

            entity.ToTable("CONTACT");

            entity.Property(e => e.ContId).HasColumnName("CONT_ID");
            entity.Property(e => e.ContAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONT_ADDRESS");
            entity.Property(e => e.ContEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONT_EMAIL");
            entity.Property(e => e.ContMessage)
                .HasColumnType("text")
                .HasColumnName("CONT_MESSAGE");
            entity.Property(e => e.ContName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CONT_NAME");
            entity.Property(e => e.ContPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CONT_PHONE");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATED_AT");
        });

        modelBuilder.Entity<Finalresult>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__FINALRES__2C6EC7C3DC54D7F1");

            entity.ToTable("FINALRESULTS");

            entity.Property(e => e.FId).HasColumnName("F_ID");
            entity.Property(e => e.FJaId).HasColumnName("F_JA_ID");
            entity.Property(e => e.FPercentage)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("F_PERCENTAGE");
            entity.Property(e => e.FTestdate)
                .HasColumnType("datetime")
                .HasColumnName("F_TESTDATE");
            entity.Property(e => e.FTotalmarksComputer).HasColumnName("F_TOTALMARKS_COMPUTER");
            entity.Property(e => e.FTotalmarksGk).HasColumnName("F_TOTALMARKS_GK");
            entity.Property(e => e.FTotalmarksMaths).HasColumnName("F_TOTALMARKS_MATHS");
            entity.Property(e => e.FTotalscoreComputer).HasColumnName("F_TOTALSCORE_COMPUTER");
            entity.Property(e => e.FTotalscoreGk).HasColumnName("F_TOTALSCORE_GK");
            entity.Property(e => e.FTotalscoreMaths).HasColumnName("F_TOTALSCORE_MATHS");
            entity.Property(e => e.FUserId).HasColumnName("F_USER_ID");
            entity.Property(e => e.FUserstatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("F_USERSTATUS");

            entity.HasOne(d => d.FJa).WithMany(p => p.Finalresults)
                .HasForeignKey(d => d.FJaId)
                .HasConstraintName("FK__FINALRESU__F_JA___7A672E12");

            entity.HasOne(d => d.FTotalmarksComputerNavigation).WithMany(p => p.Finalresults)
                .HasForeignKey(d => d.FTotalmarksComputer)
                .HasConstraintName("FK__FINALRESU__F_TOT__7E37BEF6");

            entity.HasOne(d => d.FTotalmarksGkNavigation).WithMany(p => p.Finalresults)
                .HasForeignKey(d => d.FTotalmarksGk)
                .HasConstraintName("FK__FINALRESU__F_TOT__7C4F7684");

            entity.HasOne(d => d.FTotalmarksMathsNavigation).WithMany(p => p.Finalresults)
                .HasForeignKey(d => d.FTotalmarksMaths)
                .HasConstraintName("FK__FINALRESU__F_TOT__7D439ABD");

            entity.HasOne(d => d.FUser).WithMany(p => p.Finalresults)
                .HasForeignKey(d => d.FUserId)
                .HasConstraintName("FK__FINALRESU__F_USE__7B5B524B");
        });

        modelBuilder.Entity<Gktest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GKTest__3214EC27368EC05A");

            entity.ToTable("GKTest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.OptionA)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionB)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionC)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionD)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Question).IsUnicode(false);
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.JobId).HasName("PK__Jobs__056690C2F755CDDA");

            entity.Property(e => e.CompanyName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.JobLocation)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.JobTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(e => e.JId).HasName("PK__JobAppli__92B4B6A32CE61055");

            entity.ToTable("JobApplication");

            entity.HasIndex(e => e.JEmail, "UQ__JobAppli__45E84CD4397EAF37").IsUnique();

            entity.Property(e => e.JId).HasColumnName("J_ID");
            entity.Property(e => e.JEmail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("J_EMAIL");
            entity.Property(e => e.JFullname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("J_FULLNAME");
            entity.Property(e => e.JMessage)
                .HasColumnType("text")
                .HasColumnName("J_MESSAGE");
            entity.Property(e => e.JPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("J_PHONE");
            entity.Property(e => e.JResume)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("J_RESUME");
            entity.Property(e => e.JSubmission)
                .HasColumnType("datetime")
                .HasColumnName("J_SUBMISSION");
            entity.Property(e => e.JUserId).HasColumnName("J_USER_ID");
            entity.Property(e => e.UserAddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_ADDRESS");

            entity.HasOne(d => d.JUser).WithMany(p => p.JobApplications)
                .HasForeignKey(d => d.JUserId)
                .HasConstraintName("FK__JobApplic__J_USE__778AC167");
        });

        modelBuilder.Entity<MathTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MathTest__3214EC273DFF8C1F");

            entity.ToTable("MathTest");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CorrectAnswer)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.OptionA)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionB)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionC)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OptionD)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Question).IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Role__D80AB4BBA3B71F7C");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId).HasColumnName("Role_Id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Role_Name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__USERS__F3BEEBFFA3B9A4DF");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.UserEmail, "UQ__USERS__43CA31682D1A6B5C").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("USER_ID");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_EMAIL");
            entity.Property(e => e.UserImage)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_IMAGE");
            entity.Property(e => e.UserName)
                .HasMaxLength(75)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("USER_PASSWORD");
            entity.Property(e => e.UserRoleId).HasColumnName("USER_ROLE_ID");

            entity.HasOne(d => d.UserRole).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRoleId)
                .HasConstraintName("FK__USERS__USER_ROLE__73BA3083");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
