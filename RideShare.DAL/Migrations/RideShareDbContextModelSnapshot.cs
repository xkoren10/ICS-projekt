﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RideShare.DAL;

namespace RideShare.DAL.Migrations
{
    [DbContext(typeof(RideShareDbContext))]
    partial class RideShareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RideShare.DAL.Entities.CarEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Seats")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CarEntities");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.RideEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CarId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EstEndTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Occupancy")
                        .HasColumnType("int");

                    b.Property<string>("StartLocation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("UserId");

                    b.ToTable("RideEntities");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.RideUserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("RideId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RideId");

                    b.HasIndex("UserId");

                    b.ToTable("RideUserEntities");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserEntities");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.CarEntity", b =>
                {
                    b.HasOne("RideShare.DAL.Entities.UserEntity", "User")
                        .WithMany("Cars")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("User");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.RideEntity", b =>
                {
                    b.HasOne("RideShare.DAL.Entities.CarEntity", "Car")
                        .WithMany("Rides")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RideShare.DAL.Entities.UserEntity", "User")
                        .WithMany("Rides")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Car");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.RideUserEntity", b =>
                {
                    b.HasOne("RideShare.DAL.Entities.RideEntity", "Ride")
                        .WithMany("RideUsers")
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("RideShare.DAL.Entities.UserEntity", "User")
                        .WithMany("RideUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Ride");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.CarEntity", b =>
                {
                    b.Navigation("Rides");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.RideEntity", b =>
                {
                    b.Navigation("RideUsers");
                });

            modelBuilder.Entity("RideShare.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("Cars");

                    b.Navigation("Rides");

                    b.Navigation("RideUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
