using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Gast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Plätze",
                table: "Gast");

            migrationBuilder.AddColumn<Guid>(
                name: "XFirmaId",
                table: "Gast",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirmeName",
                table: "Firma",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Gast_XFirmaId",
                table: "Gast",
                column: "XFirmaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast",
                column: "XFirmaId",
                principalTable: "Firma",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast");

            migrationBuilder.DropIndex(
                name: "IX_Gast_XFirmaId",
                table: "Gast");

            migrationBuilder.DropColumn(
                name: "XFirmaId",
                table: "Gast");

            migrationBuilder.DropColumn(
                name: "FirmeName",
                table: "Firma");

            migrationBuilder.AddColumn<string>(
                name: "Plätze",
                table: "Gast",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
