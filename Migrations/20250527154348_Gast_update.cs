using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Gast_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast");

            migrationBuilder.AddForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast",
                column: "XFirmaId",
                principalTable: "Firma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast");

            migrationBuilder.AddForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast",
                column: "XFirmaId",
                principalTable: "Firma",
                principalColumn: "Id");
        }
    }
}
