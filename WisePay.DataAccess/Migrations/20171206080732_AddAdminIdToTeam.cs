using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class AddAdminIdToTeam : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Teams",
                type: "int4",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AdminId",
                table: "Teams",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_AspNetUsers_AdminId",
                table: "Teams",
                column: "AdminId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_AspNetUsers_AdminId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_AdminId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Teams");
        }
    }
}
