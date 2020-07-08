using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Week",
                columns: table => new
                {
                    WeekId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeekNumber = table.Column<int>(nullable: false),
                    Data = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Week", x => x.WeekId);
                });

            migrationBuilder.CreateTable(
                name: "BattleFronts",
                columns: table => new
                {
                    BattleFrontId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    TotalStrenght = table.Column<int>(nullable: false),
                    WeekId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleFronts", x => x.BattleFrontId);
                    table.ForeignKey(
                        name: "FK_BattleFronts_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Armies",
                columns: table => new
                {
                    ArmyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    BattleFrontId = table.Column<int>(nullable: true),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Armies", x => x.ArmyId);
                    table.ForeignKey(
                        name: "FK_Armies_BattleFronts_BattleFrontId",
                        column: x => x.BattleFrontId,
                        principalTable: "BattleFronts",
                        principalColumn: "BattleFrontId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Armies_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Corpss",
                columns: table => new
                {
                    CorpsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    ArmyId = table.Column<int>(nullable: true),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Corpss", x => x.CorpsId);
                    table.ForeignKey(
                        name: "FK_Corpss_Armies_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Armies",
                        principalColumn: "ArmyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Corpss_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Divisions",
                columns: table => new
                {
                    DivisionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    CorpsId = table.Column<int>(nullable: true),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Divisions", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_Divisions_Corpss_CorpsId",
                        column: x => x.CorpsId,
                        principalTable: "Corpss",
                        principalColumn: "CorpsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Divisions_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Brigades",
                columns: table => new
                {
                    BrigadeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    DivisionId = table.Column<int>(nullable: true),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brigades", x => x.BrigadeId);
                    table.ForeignKey(
                        name: "FK_Brigades_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Brigades_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Regiments",
                columns: table => new
                {
                    RegimentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    BrigadeId = table.Column<int>(nullable: true),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regiments", x => x.RegimentId);
                    table.ForeignKey(
                        name: "FK_Regiments_Brigades_BrigadeId",
                        column: x => x.BrigadeId,
                        principalTable: "Brigades",
                        principalColumn: "BrigadeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Regiments_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Battalions",
                columns: table => new
                {
                    BattalionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Number = table.Column<int>(nullable: false),
                    TotalStrenght = table.Column<int>(nullable: false),
                    RegimentId = table.Column<int>(nullable: false),
                    WeekId = table.Column<int>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Battalions", x => x.BattalionId);
                    table.ForeignKey(
                        name: "FK_Battalions_Regiments_RegimentId",
                        column: x => x.RegimentId,
                        principalTable: "Regiments",
                        principalColumn: "RegimentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Battalions_Week_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Week",
                        principalColumn: "WeekId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commander",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Surname = table.Column<string>(nullable: true),
                    Paronymic = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Position = table.Column<string>(nullable: true),
                    AdditionalInformation = table.Column<string>(nullable: true),
                    BattleFrontId = table.Column<int>(nullable: true),
                    ArmyId = table.Column<int>(nullable: true),
                    CorpsId = table.Column<int>(nullable: true),
                    Division = table.Column<int>(nullable: true),
                    BrigadeId = table.Column<int>(nullable: true),
                    RegimentId = table.Column<int>(nullable: true),
                    BattalionId = table.Column<int>(nullable: true),
                    DivisionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commander", x => x.id);
                    table.ForeignKey(
                        name: "FK_Commander_Armies_ArmyId",
                        column: x => x.ArmyId,
                        principalTable: "Armies",
                        principalColumn: "ArmyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_Battalions_BattalionId",
                        column: x => x.BattalionId,
                        principalTable: "Battalions",
                        principalColumn: "BattalionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_BattleFronts_BattleFrontId",
                        column: x => x.BattleFrontId,
                        principalTable: "BattleFronts",
                        principalColumn: "BattleFrontId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_Brigades_BrigadeId",
                        column: x => x.BrigadeId,
                        principalTable: "Brigades",
                        principalColumn: "BrigadeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_Corpss_CorpsId",
                        column: x => x.CorpsId,
                        principalTable: "Corpss",
                        principalColumn: "CorpsId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commander_Regiments_RegimentId",
                        column: x => x.RegimentId,
                        principalTable: "Regiments",
                        principalColumn: "RegimentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Armies_BattleFrontId",
                table: "Armies",
                column: "BattleFrontId");

            migrationBuilder.CreateIndex(
                name: "IX_Armies_WeekId",
                table: "Armies",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Battalions_RegimentId",
                table: "Battalions",
                column: "RegimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Battalions_WeekId",
                table: "Battalions",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_BattleFronts_WeekId",
                table: "BattleFronts",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Brigades_DivisionId",
                table: "Brigades",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Brigades_WeekId",
                table: "Brigades",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_ArmyId",
                table: "Commander",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_BattalionId",
                table: "Commander",
                column: "BattalionId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_BattleFrontId",
                table: "Commander",
                column: "BattleFrontId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_BrigadeId",
                table: "Commander",
                column: "BrigadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_CorpsId",
                table: "Commander",
                column: "CorpsId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_DivisionId",
                table: "Commander",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_Commander_RegimentId",
                table: "Commander",
                column: "RegimentId");

            migrationBuilder.CreateIndex(
                name: "IX_Corpss_ArmyId",
                table: "Corpss",
                column: "ArmyId");

            migrationBuilder.CreateIndex(
                name: "IX_Corpss_WeekId",
                table: "Corpss",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_CorpsId",
                table: "Divisions",
                column: "CorpsId");

            migrationBuilder.CreateIndex(
                name: "IX_Divisions_WeekId",
                table: "Divisions",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiments_BrigadeId",
                table: "Regiments",
                column: "BrigadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Regiments_WeekId",
                table: "Regiments",
                column: "WeekId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commander");

            migrationBuilder.DropTable(
                name: "Battalions");

            migrationBuilder.DropTable(
                name: "Regiments");

            migrationBuilder.DropTable(
                name: "Brigades");

            migrationBuilder.DropTable(
                name: "Divisions");

            migrationBuilder.DropTable(
                name: "Corpss");

            migrationBuilder.DropTable(
                name: "Armies");

            migrationBuilder.DropTable(
                name: "BattleFronts");

            migrationBuilder.DropTable(
                name: "Week");
        }
    }
}
