using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Updatedb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast");

            migrationBuilder.DropColumn(
                name: "Berechnung",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "Kosten",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "Bezahltbis",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Bezahltvod",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Hotelname",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Reserviertbis",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "Reserviertvod",
                table: "Bett");

            migrationBuilder.RenameColumn(
                name: "XFirmaId",
                table: "Gast",
                newName: "XReservierungId");

            migrationBuilder.RenameIndex(
                name: "IX_Gast_XFirmaId",
                table: "Gast",
                newName: "IX_Gast_XReservierungId");

            migrationBuilder.AddColumn<Guid>(
                name: "XReservierungId",
                table: "Zimmer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Muss_bezahlen",
                table: "Reservierung",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertbis",
                table: "Reservierung",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertvod",
                table: "Reservierung",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Zahlen",
                table: "Reservierung",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "XReservierungId",
                table: "Firma",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "XReservierungId",
                table: "Bett",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tarif",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<float>(type: "real", nullable: true),
                    XHotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarif", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tarif_Hotel_XHotelId",
                        column: x => x.XHotelId,
                        principalTable: "Hotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zimmer_XReservierungId",
                table: "Zimmer",
                column: "XReservierungId");

            migrationBuilder.CreateIndex(
                name: "IX_Firma_XReservierungId",
                table: "Firma",
                column: "XReservierungId");

            migrationBuilder.CreateIndex(
                name: "IX_Bett_XReservierungId",
                table: "Bett",
                column: "XReservierungId");

            migrationBuilder.CreateIndex(
                name: "IX_Tarif_XHotelId",
                table: "Tarif",
                column: "XHotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bett_Reservierung_XReservierungId",
                table: "Bett",
                column: "XReservierungId",
                principalTable: "Reservierung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Firma_Reservierung_XReservierungId",
                table: "Firma",
                column: "XReservierungId",
                principalTable: "Reservierung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Gast_Reservierung_XReservierungId",
                table: "Gast",
                column: "XReservierungId",
                principalTable: "Reservierung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zimmer_Reservierung_XReservierungId",
                table: "Zimmer",
                column: "XReservierungId",
                principalTable: "Reservierung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bett_Reservierung_XReservierungId",
                table: "Bett");

            migrationBuilder.DropForeignKey(
                name: "FK_Firma_Reservierung_XReservierungId",
                table: "Firma");

            migrationBuilder.DropForeignKey(
                name: "FK_Gast_Reservierung_XReservierungId",
                table: "Gast");

            migrationBuilder.DropForeignKey(
                name: "FK_Zimmer_Reservierung_XReservierungId",
                table: "Zimmer");

            migrationBuilder.DropTable(
                name: "Tarif");

            migrationBuilder.DropIndex(
                name: "IX_Zimmer_XReservierungId",
                table: "Zimmer");

            migrationBuilder.DropIndex(
                name: "IX_Firma_XReservierungId",
                table: "Firma");

            migrationBuilder.DropIndex(
                name: "IX_Bett_XReservierungId",
                table: "Bett");

            migrationBuilder.DropColumn(
                name: "XReservierungId",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "Muss_bezahlen",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "Reserviertbis",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "Reserviertvod",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "Zahlen",
                table: "Reservierung");

            migrationBuilder.DropColumn(
                name: "XReservierungId",
                table: "Firma");

            migrationBuilder.DropColumn(
                name: "XReservierungId",
                table: "Bett");

            migrationBuilder.RenameColumn(
                name: "XReservierungId",
                table: "Gast",
                newName: "XFirmaId");

            migrationBuilder.RenameIndex(
                name: "IX_Gast_XReservierungId",
                table: "Gast",
                newName: "IX_Gast_XFirmaId");

            migrationBuilder.AddColumn<string>(
                name: "Berechnung",
                table: "Reservierung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "Kosten",
                table: "Reservierung",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<DateTime>(
                name: "Bezahltbis",
                table: "Bett",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Bezahltvod",
                table: "Bett",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hotelname",
                table: "Bett",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertbis",
                table: "Bett",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Reserviertvod",
                table: "Bett",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gast_Firma_XFirmaId",
                table: "Gast",
                column: "XFirmaId",
                principalTable: "Firma",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
