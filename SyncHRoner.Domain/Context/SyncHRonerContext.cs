using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using SyncHRoner.Common.Functional;
using SyncHRoner.Domain.Base;
using SyncHRoner.Domain.Entities;
using SyncHRoner.Domain.ValueObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static SyncHRoner.Common.Functional.FuncHelpers;

namespace SyncHRoner.Domain.Context
{
    public class SyncHRonerContext : DbContext
    {
        public SyncHRonerContext()
        {

        }

        public SyncHRonerContext(DbContextOptions<SyncHRonerContext> options)
            : base(options)
        {
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-8IUJEPL;Initial Catalog=SyncHRoner;Integrated Security=True");

        }

        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Gender> Gender { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Profiles");

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.ToTable("Profile", "Profiles");

                entity.Property(e => e.Id)
                      .HasColumnName("ProfileId")
                      .UseIdentityColumn();

                entity.OwnsOne(p => p.FullName, p =>
                {
                    p.Property(pp => pp.FirstName).HasColumnName("FirstName").IsRequired().HasMaxLength(50);
                    p.Property(pp => pp.LastName).HasColumnName("LastName").IsRequired().HasMaxLength(50);
                });

                entity.Property(e => e.BirthDate)
                       .HasConversion(p => p.Value, p => BirthDate.Create(p).GetSuccessValue())
                       .IsRequired();

                entity.Property(e => e.CreationDate)
                      .IsRequired();

                entity.Property(e => e.LastUpdate);

                entity.Property<long>("GenderId")
                       .HasColumnType("bigint")
                       .IsRequired();

                entity.Property(e => e.Phone)
                       .HasConversion(p => p.Value, p => Phone.Create(p).GetSuccessValue())
                       .HasMaxLength(11)
                       .IsRequired();

                entity.Property(e => e.Email)
                       .HasConversion(p => p.Value, p => Email.Create(p).GetSuccessValue())
                       .HasMaxLength(255)
                       .IsRequired();

                entity.Property(e => e.Rating)
                       .HasConversion(p => p.Value, p => Rating.Create(p).GetSuccessValue())
                       .IsRequired();

                entity.Property(e => e.IsActive)
                      .IsRequired();


                entity.HasMany(e => e.ProfileEmails)
                      .WithOne()
                      .HasForeignKey("ProfileId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

                entity.HasMany(e => e.ProfilePhones)
                      .WithOne()
                      .HasForeignKey("ProfileId")
                      .OnDelete(DeleteBehavior.Cascade)
                      .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);


                entity.HasMany(e => e.NationalitiesLink)
                      .WithOne()
                      .HasForeignKey("ProfileId");

                //only one profile should be assigned to some email
                //also is an alternate key as long as we do not allow duplicate emails across users
                entity.HasIndex(e => e.Email)
                      .IsUnique()
                      .HasName("AK_Profile_Email");

                //only one phone should be assigned to some email
                //also is an alternate key as long as we do not allow duplicate phones across users
                entity.HasIndex(e => e.Phone)
                      .IsUnique()
                      .HasName("AK_Profile_Phone");

            });


            modelBuilder.Entity<ProfileNationality>(entity =>
            {
                entity.ToTable("ProfileNationality", "Profiles");

                entity.Property<long>("ProfileId")
                      .HasColumnType("bigint")
                      .IsRequired();

                entity.Property<long>("CountryId")
                      .HasColumnType("bigint")
                      .IsRequired();

                entity.HasKey(new[] { "ProfileId", "CountryId" });
            });

            modelBuilder.Entity<ProfileEmail>(entity =>
            {
                entity.ToTable("ProfileEmail", "Profiles");

                entity.Property(e => e.Id)
                       .HasColumnName("ProfileEmailId")
                       .UseIdentityColumn();

                entity.Property(e => e.Email)
                       .HasConversion(p => p.Value, p => Email.Create(p).GetSuccessValue())
                       .IsRequired()
                       .HasMaxLength(255);

                entity.Property<long>("ProfileId")
                      .HasColumnType("bigint")
                      .IsRequired();
            });

            modelBuilder.Entity<ProfilePhone>(entity =>
            {
                entity.ToTable("ProfilePhone", "Profiles");

                entity.Property(e => e.Id)
                       .HasColumnName("ProfilePhoneId")
                       .UseIdentityColumn();

                entity.Property(e => e.Phone)
                       .HasConversion(p => p.Value, p => Phone.Create(p).GetSuccessValue())
                       .IsRequired()
                       .HasMaxLength(11);

                entity.Property<long>("ProfileId")
                      .HasColumnType("bigint")
                      .IsRequired();
            });

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender", "Profiles");

                entity.Property(e => e.Id)
                       .HasColumnName("GenderId")
                       .ValueGeneratedNever();


                entity.HasMany(typeof(Profile))
                      .WithOne()
                      .HasForeignKey("GenderId")
                      .IsRequired();

                entity.Property(e => e.Name)
                       .IsRequired()
                       .HasMaxLength(10);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Country", "Profiles");

                entity.Property(e => e.Id)
                       .HasColumnName("CountryId")
                       .ValueGeneratedNever();

                entity.Property(e => e.Name)
                       .IsRequired()
                       .HasMaxLength(60);


                entity.HasMany(e => e.ProfilesLink)
                      .WithOne()
                      .HasForeignKey("CountryId");
            });
        }

    }
}
