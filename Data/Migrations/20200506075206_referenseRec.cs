using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class referenseRec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorpsId",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BattlefrontId",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorpsId",
                table: "Battalions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regiments_CorpsId",
                table: "Regiments",
                column: "CorpsId");

            migrationBuilder.CreateIndex(
                name: "IX_Corpss_BattlefrontId",
                table: "Corpss",
                column: "BattlefrontId");

            migrationBuilder.CreateIndex(
                name: "IX_Battalions_CorpsId",
                table: "Battalions",
                column: "CorpsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battalions_Corpss_CorpsId",
                table: "Battalions",
                column: "CorpsId",
                principalTable: "Corpss",
                principalColumn: "CorpsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Corpss_BattleFronts_BattlefrontId",
                table: "Corpss",
                column: "BattlefrontId",
                principalTable: "BattleFronts",
                principalColumn: "BattleFrontId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regiments_Corpss_CorpsId",
                table: "Regiments",
                column: "CorpsId",
                principalTable: "Corpss",
                principalColumn: "CorpsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battalions_Corpss_CorpsId",
                table: "Battalions");

            migrationBuilder.DropForeignKey(
                name: "FK_Corpss_BattleFronts_BattlefrontId",
                table: "Corpss");

            migrationBuilder.DropForeignKey(
                name: "FK_Regiments_Corpss_CorpsId",
                table: "Regiments");

            migrationBuilder.DropIndex(
                name: "IX_Regiments_CorpsId",
                table: "Regiments");

            migrationBuilder.DropIndex(
                name: "IX_Corpss_BattlefrontId",
                table: "Corpss");

            migrationBuilder.DropIndex(
                name: "IX_Battalions_CorpsId",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "CorpsId",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "BattlefrontId",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "CorpsId",
                table: "Battalions");
        }
    }
}
