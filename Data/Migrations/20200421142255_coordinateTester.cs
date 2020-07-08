using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class coordinateTester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Armies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Armies");
        }
    }
}
