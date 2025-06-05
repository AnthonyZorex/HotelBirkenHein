using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Zimmer_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnzyhlBetten",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "Belegt",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "Betten",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "Frei",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "FreiBetten",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "ReserviertBetten",
                table: "Zimmer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnzyhlBetten",
                table: "Zimmer",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Belegt",
                table: "Zimmer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Betten",
                table: "Zimmer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Frei",
                table: "Zimmer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FreiBetten",
                table: "Zimmer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReserviertBetten",
                table: "Zimmer",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
