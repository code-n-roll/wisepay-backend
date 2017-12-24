using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class UseOneToOneRelationForPurchaseItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPurchaseItem_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPurchaseItem",
                table: "UserPurchaseItem");

            migrationBuilder.DropColumn(
                name: "Sum",
                table: "UserPurchaseItem");

            migrationBuilder.RenameTable(
                name: "UserPurchaseItem",
                newName: "UserPurchaseItems");

            migrationBuilder.RenameIndex(
                name: "IX_UserPurchaseItem_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems",
                newName: "IX_UserPurchaseItems_UserPurchaseUserId_UserPurchasePurchaseId");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "UserPurchaseItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "UserPurchaseItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPurchaseItems",
                table: "UserPurchaseItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems",
                columns: new[] { "UserPurchaseUserId", "UserPurchasePurchaseId" },
                principalTable: "UserPurchases",
                principalColumns: new[] { "UserId", "PurchaseId" },
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserPurchaseItems",
                table: "UserPurchaseItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "UserPurchaseItems");

            migrationBuilder.RenameTable(
                name: "UserPurchaseItems",
                newName: "UserPurchaseItem");

            migrationBuilder.RenameIndex(
                name: "IX_UserPurchaseItems_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItem",
                newName: "IX_UserPurchaseItem_UserPurchaseUserId_UserPurchasePurchaseId");

            migrationBuilder.AlterColumn<long>(
                name: "ItemId",
                table: "UserPurchaseItem",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Sum",
                table: "UserPurchaseItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserPurchaseItem",
                table: "UserPurchaseItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPurchaseItem_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItem",
                columns: new[] { "UserPurchaseUserId", "UserPurchasePurchaseId" },
                principalTable: "UserPurchases",
                principalColumns: new[] { "UserId", "PurchaseId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
