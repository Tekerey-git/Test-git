﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace HistorySiteIdentity.Data.Migrations
{
    public partial class ForAdress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Armies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Armies");
        }
    }
}
