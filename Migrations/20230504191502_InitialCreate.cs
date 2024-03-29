﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HallManagementTest2.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiefHallAdmins",
                columns: table => new
                {
                    ChiefHallAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiefHallAdmins", x => x.ChiefHallAdminId);
                });

            migrationBuilder.CreateTable(
                name: "HallAdmins",
                columns: table => new
                {
                    HallAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallAdmins", x => x.HallAdminId);
                });

            migrationBuilder.CreateTable(
                name: "HallTypes",
                columns: table => new
                {
                    HallTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallCount = table.Column<int>(type: "int", nullable: false),
                    RoomSpaceCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HallTypes", x => x.HallTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Halls",
                columns: table => new
                {
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    AvailableRooms = table.Column<int>(type: "int", nullable: false),
                    StudentCount = table.Column<int>(type: "int", nullable: false),
                    RoomSpace = table.Column<int>(type: "int", nullable: false),
                    BlockCount = table.Column<int>(type: "int", nullable: false),
                    HallGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAssigned = table.Column<bool>(type: "bit", nullable: false),
                    HallTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Halls", x => x.HallId);
                    table.ForeignKey(
                        name: "FK_Halls_HallTypes_HallTypeId",
                        column: x => x.HallTypeId,
                        principalTable: "HallTypes",
                        principalColumn: "HallTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomCount = table.Column<int>(type: "int", nullable: false),
                    AvailableRooms = table.Column<int>(type: "int", nullable: false),
                    StudentCount = table.Column<int>(type: "int", nullable: false),
                    RoomSpace = table.Column<int>(type: "int", nullable: false),
                    BlockGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.BlockId);
                    table.ForeignKey(
                        name: "FK_Blocks_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComplaintForms",
                columns: table => new
                {
                    ComplaintFormId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Plumbing = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Carpentary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Electrical = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Others = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComplaintForms", x => x.ComplaintFormId);
                    table.ForeignKey(
                        name: "FK_ComplaintForms_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotiFicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificationContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotiFicationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Porters",
                columns: table => new
                {
                    PorterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Porters", x => x.PorterId);
                    table.ForeignKey(
                        name: "FK_Porters_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxOccupants = table.Column<int>(type: "int", nullable: false),
                    AvailableSpace = table.Column<int>(type: "int", nullable: false),
                    StudentCount = table.Column<int>(type: "int", nullable: false),
                    RoomGender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnderMaintenance = table.Column<bool>(type: "bit", nullable: false),
                    IsFull = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomId);
                    table.ForeignKey(
                        name: "FK_Rooms_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "BlockId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudyLevel = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MatricNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BlockId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    AccessToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "BlockId");
                    table.ForeignKey(
                        name: "FK_Students_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId");
                    table.ForeignKey(
                        name: "FK_Students_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId");
                });

            migrationBuilder.CreateTable(
                name: "ExitPasses",
                columns: table => new
                {
                    ExitPassId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfExit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfReturn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReasonForLeaving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateOfArrival = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasReturned = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExitPasses", x => x.ExitPassId);
                    table.ForeignKey(
                        name: "FK_ExitPasses_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExitPasses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentDevices",
                columns: table => new
                {
                    StudentDeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HallId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatricNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Item = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDevices", x => x.StudentDeviceId);
                    table.ForeignKey(
                        name: "FK_StudentDevices_Halls_HallId",
                        column: x => x.HallId,
                        principalTable: "Halls",
                        principalColumn: "HallId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDevices_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_HallId",
                table: "Blocks",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_ComplaintForms_HallId",
                table: "ComplaintForms",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_ExitPasses_HallId",
                table: "ExitPasses",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_ExitPasses_StudentId",
                table: "ExitPasses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Halls_HallTypeId",
                table: "Halls",
                column: "HallTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_HallId",
                table: "Notifications",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Porters_HallId",
                table: "Porters",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_BlockId",
                table: "Rooms",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HallId",
                table: "Rooms",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDevices_HallId",
                table: "StudentDevices",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDevices_StudentId",
                table: "StudentDevices",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_BlockId",
                table: "Students",
                column: "BlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_HallId",
                table: "Students",
                column: "HallId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_RoomId",
                table: "Students",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiefHallAdmins");

            migrationBuilder.DropTable(
                name: "ComplaintForms");

            migrationBuilder.DropTable(
                name: "ExitPasses");

            migrationBuilder.DropTable(
                name: "HallAdmins");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Porters");

            migrationBuilder.DropTable(
                name: "StudentDevices");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "Halls");

            migrationBuilder.DropTable(
                name: "HallTypes");
        }
    }
}
