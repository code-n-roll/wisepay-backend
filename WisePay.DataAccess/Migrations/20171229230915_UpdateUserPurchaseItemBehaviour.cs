using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class UpdateUserPurchaseItemBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems",
                columns: new[] { "UserPurchaseUserId", "UserPurchasePurchaseId" },
                principalTable: "UserPurchases",
                principalColumns: new[] { "UserId", "PurchaseId" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPurchaseItems_UserPurchases_UserPurchaseUserId_UserPurchasePurchaseId",
                table: "UserPurchaseItems",
                columns: new[] { "UserPurchaseUserId", "UserPurchasePurchaseId" },
                principalTable: "UserPurchases",
                principalColumns: new[] { "UserId", "PurchaseId" },
                onDelete: ReferentialAction.Restrict);
        }
    }
}
