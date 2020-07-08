using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class viewbagId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DivisionId",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArmyId",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ArmyId",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CorpsId",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "BattleFronts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "BattleFronts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "BattleFronts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "BattleFronts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Battalions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrigadeId",
                table: "Battalions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordX",
                table: "Battalions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordY",
                table: "Battalions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoordinatesXY",
                table: "Battalions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regiments_DivisionId",
                table: "Regiments",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_ArmyId",
                table: "Divisions",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_Brigades_ArmyId",
                table: "Brigades",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_Brigades_CorpsId",
                table: "Brigades",
                column: "CorpsId");

            migrationBuilder.CreateIndex(
                name: "IX_Battalions_BrigadeId",
                table: "Battalions",
                column: "BrigadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Battalions_Brigades_BrigadeId",
                table: "Battalions",
                column: "BrigadeId",
                principalTable: "Brigades",
                principalColumn: "BrigadeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Brigades_Armies_ArmyId",
                table: "Brigades",
                column: "ArmyId",
                principalTable: "Armies",
                principalColumn: "ArmyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Brigades_Corpss_CorpsId",
                table: "Brigades",
                column: "CorpsId",
                principalTable: "Corpss",
                principalColumn: "CorpsId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Divisions_Armies_ArmyId",
                table: "Divisions",
                column: "ArmyId",
                principalTable: "Armies",
                principalColumn: "ArmyId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regiments_Divisions_DivisionId",
                table: "Regiments",
                column: "DivisionId",
                principalTable: "Divisions",
                principalColumn: "DivisionId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Battalions_Brigades_BrigadeId",
                table: "Battalions");

            migrationBuilder.DropForeignKey(
                name: "FK_Brigades_Armies_ArmyId",
                table: "Brigades");

            migrationBuilder.DropForeignKey(
                name: "FK_Brigades_Corpss_CorpsId",
                table: "Brigades");

            migrationBuilder.DropForeignKey(
                name: "FK_Divisions_Armies_ArmyId",
                table: "Divisions");

            migrationBuilder.DropForeignKey(
                name: "FK_Regiments_Divisions_DivisionId",
                table: "Regiments");

            migrationBuilder.DropIndex(
                name: "IX_Regiments_DivisionId",
                table: "Regiments");

            migrationBuilder.DropIndex(
                name: "IX_Divisions_ArmyId",
                table: "Divisions");

            migrationBuilder.DropIndex(
                name: "IX_Brigades_ArmyId",
                table: "Brigades");

            migrationBuilder.DropIndex(
                name: "IX_Brigades_CorpsId",
                table: "Brigades");

            migrationBuilder.DropIndex(
                name: "IX_Battalions_BrigadeId",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "DivisionId",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "ArmyId",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "ArmyId",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "CorpsId",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "BattleFronts");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "BattleFronts");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "BattleFronts");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "BattleFronts");

            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "BrigadeId",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "CoordX",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "CoordY",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "CoordinatesXY",
                table: "Battalions");
        }
    }
}
