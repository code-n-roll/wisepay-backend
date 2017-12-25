using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class AddStoreOrdersModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Purchases",
                type: "int4",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StoreOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsSubmitted = table.Column<bool>(type: "bool", nullable: false),
                    PurchaseId = table.Column<int>(type: "int4", nullable: false),
                    StoreId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreOrders_Purchases_PurchaseId",
                        column: x => x.PurchaseId,
                        principalTable: "Purchases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPurchaseItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int4", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    ItemId = table.Column<long>(type: "int8", nullable: false),
                    Number = table.Column<long>(type: "int8", nullable: false),
                    Sum = table.Column<decimal>(type: "numeric", nullable: false),
                    UserPurchasePurchaseId = table.Column<int>(type: "int4", nullable: true),
                    UserPurchaseUserId = table.Column<int>(type: "int4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPurchaseItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPurchaseItem_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                        columns: x => new { x.UserPurchaseUserId, x.UserPurchasePurchaseId },
                        principalTable: "UserPurchases",
                        principalColumns: new[] { "UserId", "PurchaseId" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreOrders_PurchaseId",
                table: "StoreOrders",
                column: "PurchaseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPurchaseItem_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItem",
                columns: new[] { "UserPurchaseUserId", "UserPurchasePurchaseId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreOrders");

            migrationBuilder.DropTable(
                name: "UserPurchaseItem");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Purchases");
        }
    }
}
