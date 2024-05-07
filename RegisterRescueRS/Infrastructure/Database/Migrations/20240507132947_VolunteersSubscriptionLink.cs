using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RegisterRescueRS.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class VolunteersSubscriptionLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VolunteersSubscriptionLink",
                table: "ShelterNeeds",
                type: "NVARCHAR2(2000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VolunteersSubscriptionLink",
                table: "ShelterNeeds");
        }
    }
}
