using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Hotel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BettName",
                table: "Bett",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertbis",
                table: "Bett",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertvod",
                table: "Bett",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Firma",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HRB = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bemekung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kunden_Nr = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firma", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gast",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StAG = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Geschlecht = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prvat_or_firma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kunden_Nr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Plätze = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gast", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservierung",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Zahlungsart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kosten = table.Column<float>(type: "real", nullable: false),
                    Berechnung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rechnungsempf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kontakt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameGast = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GastKontakt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bemerkung = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservierung", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Firma");

            migrationBuilder.DropTable(
                name: "Gast");

            migrationBuilder.DropTable(
                name: "Reservierung");

            migrationBuilder.DropColumn(
                name: "BettName",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Reserviertbis",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Reserviertvod",
                table: "Bett");
        }
    }
}
