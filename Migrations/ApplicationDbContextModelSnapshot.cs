﻿// <auto-generated />
using System;
using HallManagementTest2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HallManagementTest2.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HallManagementTest2.Models.Block", b =>
                {
                    b.Property<Guid>("BlockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableRooms")
                        .HasColumnType("int");

                    b.Property<string>("BlockGender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BlockName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("RoomCount")
                        .HasColumnType("int");

                    b.Property<int>("RoomSpace")
                        .HasColumnType("int");

                    b.Property<int>("StudentCount")
                        .HasColumnType("int");

                    b.HasKey("BlockId");

                    b.HasIndex("HallId");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("HallManagementTest2.Models.ChiefHallAdmin", b =>
                {
                    b.Property<Guid>("ChiefHallAdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChiefHallAdminId");

                    b.ToTable("ChiefHallAdmins");
                });

            modelBuilder.Entity("HallManagementTest2.Models.ComplaintForm", b =>
                {
                    b.Property<Guid>("ComplaintFormId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Carpentary")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Electrical")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Others")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Plumbing")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RoomNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ComplaintFormId");

                    b.HasIndex("HallId");

                    b.ToTable("ComplaintForms");
                });

            modelBuilder.Entity("HallManagementTest2.Models.ExitPass", b =>
                {
                    b.Property<Guid>("ExitPassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateIssued")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfExit")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfReturn")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("HasReturned")
                        .HasColumnType("bit");

                    b.Property<string>("ReasonForLeaving")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StateOfArrival")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ExitPassId");

                    b.HasIndex("HallId");

                    b.HasIndex("StudentId");

                    b.ToTable("ExitPasses");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Hall", b =>
                {
                    b.Property<Guid>("HallId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableRooms")
                        .HasColumnType("int");

                    b.Property<int>("BlockCount")
                        .HasColumnType("int");

                    b.Property<string>("HallGender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HallName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsAssigned")
                        .HasColumnType("bit");

                    b.Property<int>("RoomCount")
                        .HasColumnType("int");

                    b.Property<int>("RoomSpace")
                        .HasColumnType("int");

                    b.Property<int>("StudentCount")
                        .HasColumnType("int");

                    b.HasKey("HallId");

                    b.HasIndex("HallTypeId");

                    b.ToTable("Halls");
                });

            modelBuilder.Entity("HallManagementTest2.Models.HallAdmin", b =>
                {
                    b.Property<Guid>("HallAdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HallAdminId");

                    b.ToTable("HallAdmins");
                });

            modelBuilder.Entity("HallManagementTest2.Models.HallType", b =>
                {
                    b.Property<Guid>("HallTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("HallCount")
                        .HasColumnType("int");

                    b.Property<int>("RoomSpaceCount")
                        .HasColumnType("int");

                    b.HasKey("HallTypeId");

                    b.ToTable("HallTypes");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Notification", b =>
                {
                    b.Property<Guid>("NotiFicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NotificationContent")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NotiFicationId");

                    b.HasIndex("HallId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Porter", b =>
                {
                    b.Property<Guid>("PorterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PorterId");

                    b.HasIndex("HallId");

                    b.ToTable("Porters");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Room", b =>
                {
                    b.Property<Guid>("RoomId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AvailableSpace")
                        .HasColumnType("int");

                    b.Property<Guid>("BlockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsFull")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUnderMaintenance")
                        .HasColumnType("bit");

                    b.Property<int>("MaxOccupants")
                        .HasColumnType("int");

                    b.Property<string>("RoomGender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StudentCount")
                        .HasColumnType("int");

                    b.HasKey("RoomId");

                    b.HasIndex("BlockId");

                    b.HasIndex("HallId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("BlockId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Course")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatricNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("StudyLevel")
                        .HasColumnType("int");

                    b.Property<DateTime>("TokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TokenExpires")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.HasIndex("BlockId");

                    b.HasIndex("HallId");

                    b.HasIndex("RoomId");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("HallManagementTest2.Models.StudentDevice", b =>
                {
                    b.Property<Guid>("StudentDeviceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("HallId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<string>("Item")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MatricNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StudentDeviceId");

                    b.HasIndex("HallId");

                    b.HasIndex("StudentId");

                    b.ToTable("StudentDevices");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Block", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("Blocks")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.ComplaintForm", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("ComplaintForms")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.ExitPass", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("ExitPasses")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HallManagementTest2.Models.Student", null)
                        .WithMany("ExitPasses")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.Hall", b =>
                {
                    b.HasOne("HallManagementTest2.Models.HallType", null)
                        .WithMany("Halls")
                        .HasForeignKey("HallTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.Notification", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("Notifications")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.Porter", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("Porters")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.Room", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Block", null)
                        .WithMany("Rooms")
                        .HasForeignKey("BlockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("Rooms")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HallManagementTest2.Models.Student", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Block", null)
                        .WithMany("Students")
                        .HasForeignKey("BlockId");

                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("Students")
                        .HasForeignKey("HallId");

                    b.HasOne("HallManagementTest2.Models.Room", null)
                        .WithMany("Students")
                        .HasForeignKey("RoomId");
                });

            modelBuilder.Entity("HallManagementTest2.Models.StudentDevice", b =>
                {
                    b.HasOne("HallManagementTest2.Models.Hall", null)
                        .WithMany("StudentDevices")
                        .HasForeignKey("HallId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HallManagementTest2.Models.Student", null)
                        .WithMany("StudentDevices")
                        .HasForeignKey("StudentId");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Block", b =>
                {
                    b.Navigation("Rooms");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Hall", b =>
                {
                    b.Navigation("Blocks");

                    b.Navigation("ComplaintForms");

                    b.Navigation("ExitPasses");

                    b.Navigation("Notifications");

                    b.Navigation("Porters");

                    b.Navigation("Rooms");

                    b.Navigation("StudentDevices");

                    b.Navigation("Students");
                });

            modelBuilder.Entity("HallManagementTest2.Models.HallType", b =>
                {
                    b.Navigation("Halls");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Room", b =>
                {
                    b.Navigation("Students");
                });

            modelBuilder.Entity("HallManagementTest2.Models.Student", b =>
                {
                    b.Navigation("ExitPasses");

                    b.Navigation("StudentDevices");
                });
#pragma warning restore 612, 618
        }
    }
}
