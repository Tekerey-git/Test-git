using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class nullableMigr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander");

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Commander",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "WeekId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander");

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Commander",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "WeekId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
