using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class NoNameNew2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Regiments",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Divisions",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Corpss",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Brigades",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "BattleFronts",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Battalions",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Armies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Regiments");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Divisions");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Corpss");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "BattleFronts");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Battalions");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Armies");
        }
    }
}
