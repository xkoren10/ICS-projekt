using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RideShare.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Seats = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RideEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Occupancy = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideEntities_CarEntities_CarId",
                        column: x => x.CarId,
                        principalTable: "CarEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RideEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RideUserEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RideId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RideUserEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RideUserEntities_RideEntities_RideId",
                        column: x => x.RideId,
                        principalTable: "RideEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RideUserEntities_UserEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarEntities_UserId",
                table: "CarEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideEntities_CarId",
                table: "RideEntities",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_RideEntities_UserId",
                table: "RideEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RideUserEntities_RideId",
                table: "RideUserEntities",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideUserEntities_UserId",
                table: "RideUserEntities",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RideUserEntities");

            migrationBuilder.DropTable(
                name: "RideEntities");

            migrationBuilder.DropTable(
                name: "CarEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");
        }
    }
}
