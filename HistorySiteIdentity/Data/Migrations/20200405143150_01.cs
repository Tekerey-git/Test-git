using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class _01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander");

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Commander",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "WeekId",
                table: "Commander",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Commander_Week_WeekId",
                table: "Commander",
                column: "WeekId",
                principalTable: "Week",
                principalColumn: "WeekId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
