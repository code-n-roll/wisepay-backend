using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WisePay.DataAccess.Migrations
{
    public partial class RenamePaymentHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PayHistory",
                newName: "PaymentHistory");

            migrationBuilder.RenameSequence(
                name: "PayHistory_Id_seq",
                newName: "PaymentHistory_Id_seq");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "PaymentHistory",
                newName: "PayHistory");

            migrationBuilder.RenameTable(
                name: "PaymentHistory_Id_seq",
                newName: "PayHistory_Id_seq");
        }
    }
}
