using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterRescueRS.Migrations
{
    /// <inheritdoc />
    public partial class AdmShelter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Adm",
                table: "Shelters",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adm",
                table: "Shelters");
        }
    }
}
