using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class Second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeekId",
                table: "Commander",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Commander_WeekId",
                table: "Commander",
                column: "WeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "WeekId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander");

            migrationBuilder.DropIndex(
                name: "IX_Commander_WeekId",
                table: "Commander");

            migrationBuilder.DropColumn(
                name: "WeekId",
                table: "Commander");
        }
    }
}
