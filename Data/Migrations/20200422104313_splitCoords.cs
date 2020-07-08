using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class splitCoords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "Armies");

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Armies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Armies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Armies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Armies");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Armies");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Armies");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "Armies",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
