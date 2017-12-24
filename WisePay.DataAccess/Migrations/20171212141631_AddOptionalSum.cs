using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class AddOptionalSum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayedOff",
                table: "UserPurchases");

            migrationBuilder.DropColumn(
                name: "BankToken",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "UserPurchases",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserPurchases",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BankActionToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankIdToken",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    PurchaseId = table.Column<int>(type: "int4", nullable: false),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp", nullable: false),
                    UserId = table.Column<int>(type: "int4", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayHistory_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PayHistory_PurchaseId",
                table: "PayHistory",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PayHistory_UserId",
                table: "PayHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayHistory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserPurchases");

            migrationBuilder.DropColumn(
                name: "BankActionToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BankIdToken",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Sum",
                table: "UserPurchases",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPayedOff",
                table: "UserPurchases",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BankToken",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
