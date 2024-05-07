using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterRescueRS.Migrations
{
    /// <inheritdoc />
    public partial class ShelterNeedsAvaliable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShelterNeeds_ShelterId",
                table: "ShelterNeeds");

            migrationBuilder.AddColumn<int>(
                name: "Avaliable",
                table: "ShelterNeeds",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ShelterNeeds_ShelterId",
                table: "ShelterNeeds",
                column: "ShelterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShelterNeeds_ShelterId",
                table: "ShelterNeeds");

            migrationBuilder.DropColumn(
                name: "Avaliable",
                table: "ShelterNeeds");

            migrationBuilder.CreateIndex(
                name: "IX_ShelterNeeds_ShelterId",
                table: "ShelterNeeds",
                column: "ShelterId",
                unique: true);
        }
    }
}
