using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class lastOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battalions_Regiments_RegimentId",
                table: "Battalions");

            migrationBuilder.AlterColumn<int>(
                name: "RegimentId",
                table: "Battalions",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Battalions_Regiments_RegimentId",
                table: "Battalions",
                column: "RegimentId",
                principalTable: "Regiments",
                principalColumn: "RegimentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battalions_Regiments_RegimentId",
                table: "Battalions");

            migrationBuilder.AlterColumn<int>(
                name: "RegimentId",
                table: "Battalions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Battalions_Regiments_RegimentId",
                table: "Battalions",
                column: "RegimentId",
                principalTable: "Regiments",
                principalColumn: "RegimentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
