﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;
using RegisterRescueRS.Infrastructure.Database;

#nullable disable

namespace RegisterRescueRS.Infrastructure.Database.Migrations
{
    [DbContext(typeof(RegisterRescueRSDbContext))]
    partial class RegisterRescueRSDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.FamilyEntity", b =>
                {
                    b.Property<Guid>("FamilyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<DateTimeOffset>("RegisteredAt")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<Guid>("ResponsableId")
                        .HasColumnType("RAW(16)");

                    b.Property<Guid>("ShelterId")
                        .HasColumnType("RAW(16)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.HasKey("FamilyId");

                    b.HasIndex("ResponsableId")
                        .IsUnique();

                    b.HasIndex("ShelterId");

                    b.ToTable("Families");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.HousedEntity", b =>
                {
                    b.Property<Guid>("HousedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<int>("Age")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Cellphone")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<Guid>("FamilyId")
                        .HasColumnType("RAW(16)");

                    b.Property<Guid>("FamilyResponsableId")
                        .HasColumnType("RAW(16)");

                    b.Property<int>("IsFamilyResponsable")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTimeOffset>("RegisteredAt")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.HasKey("HousedId");

                    b.HasIndex("FamilyId");

                    b.ToTable("Houseds");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.ShelterEntity", b =>
                {
                    b.Property<Guid>("ShelterId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ShelterName")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("ShelterId");

                    b.ToTable("Shelters");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.ShelterNeedsEntity", b =>
                {
                    b.Property<Guid>("ShelterNeedsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<int>("AcceptingDoctors")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("AcceptingDonations")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("AcceptingVeterinarians")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("AcceptingVolunteers")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("DonationDescription")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<Guid>("ShelterId")
                        .HasColumnType("RAW(16)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<string>("VolunteersSubscriptionLink")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("ShelterNeedsId");

                    b.ToTable("ShelterNeeds");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.FamilyEntity", b =>
                {
                    b.HasOne("RegisterRescueRS.Domain.Application.Entities.HousedEntity", "Responsable")
                        .WithOne("FamilyResponsable")
                        .HasForeignKey("RegisterRescueRS.Domain.Application.Entities.FamilyEntity", "ResponsableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RegisterRescueRS.Domain.Application.Entities.ShelterEntity", "Shelter")
                        .WithMany("Families")
                        .HasForeignKey("ShelterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Responsable");

                    b.Navigation("Shelter");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.HousedEntity", b =>
                {
                    b.HasOne("RegisterRescueRS.Domain.Application.Entities.FamilyEntity", "Family")
                        .WithMany("Houseds")
                        .HasForeignKey("FamilyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Family");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.ShelterEntity", b =>
                {
                    b.HasOne("RegisterRescueRS.Domain.Application.Entities.ShelterNeedsEntity", "ShelterNeeds")
                        .WithOne("Shelter")
                        .HasForeignKey("RegisterRescueRS.Domain.Application.Entities.ShelterEntity", "ShelterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShelterNeeds");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.FamilyEntity", b =>
                {
                    b.Navigation("Houseds");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.HousedEntity", b =>
                {
                    b.Navigation("FamilyResponsable");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.ShelterEntity", b =>
                {
                    b.Navigation("Families");
                });

            modelBuilder.Entity("RegisterRescueRS.Domain.Application.Entities.ShelterNeedsEntity", b =>
                {
                    b.Navigation("Shelter")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
