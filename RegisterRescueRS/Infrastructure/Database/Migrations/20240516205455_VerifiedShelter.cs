using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterRescueRS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class VerifiedShelter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ShelterCellphone",
                table: "Shelters",
                type: "NVARCHAR2(2000)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Verified",
                table: "Shelters",
                type: "NUMBER(10)",
                nullable: true,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShelterCellphone",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Shelters");
        }
    }
}
