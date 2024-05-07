using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterRescueRS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class ShelterNeeds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Shelters",
                type: "NVARCHAR2(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ShelterNeeds",
                columns: table => new
                {
                    ShelterNeedsId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ShelterId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    AcceptingVolunteers = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AcceptingDoctors = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AcceptingVeterinarians = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    AcceptingDonations = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DonationDescription = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShelterNeeds", x => x.ShelterNeedsId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Shelters_ShelterNeeds_ShelterId",
                table: "Shelters",
                column: "ShelterId",
                principalTable: "ShelterNeeds",
                principalColumn: "ShelterNeedsId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelters_ShelterNeeds_ShelterId",
                table: "Shelters");

            migrationBuilder.DropTable(
                name: "ShelterNeeds");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Shelters");
        }
    }
}
