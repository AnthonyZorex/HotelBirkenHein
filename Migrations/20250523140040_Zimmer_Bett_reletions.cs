using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Zimmer_Bett_reletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bett_Zimmer_ZimmerId",
                table: "Bett");

            migrationBuilder.RenameColumn(
                name: "ZimmerId",
                table: "Bett",
                newName: "XZimmerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bett_ZimmerId",
                table: "Bett",
                newName: "IX_Bett_XZimmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bett_Zimmer_XZimmerId",
                table: "Bett",
                column: "XZimmerId",
                principalTable: "Zimmer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bett_Zimmer_XZimmerId",
                table: "Bett");

            migrationBuilder.RenameColumn(
                name: "XZimmerId",
                table: "Bett",
                newName: "ZimmerId");

            migrationBuilder.RenameIndex(
                name: "IX_Bett_XZimmerId",
                table: "Bett",
                newName: "IX_Bett_ZimmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bett_Zimmer_ZimmerId",
                table: "Bett",
                column: "ZimmerId",
                principalTable: "Zimmer",
                principalColumn: "Id");
        }
    }
}
