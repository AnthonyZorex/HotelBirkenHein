using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Hotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kontaktperson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Private = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zimmer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Zimmernummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnzyhlBetten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Belegt = table.Column<int>(type: "int", nullable: false),
                    Frei = table.Column<int>(type: "int", nullable: false),
                    Betten = table.Column<int>(type: "int", nullable: false),
                    ReserviertBetten = table.Column<int>(type: "int", nullable: false),
                    FreiBetten = table.Column<int>(type: "int", nullable: false),
                    XHotelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    XHotelsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zimmer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zimmer_Hotel_XHotelsId",
                        column: x => x.XHotelsId,
                        principalTable: "Hotel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bett",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ZimmerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bett", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bett_Zimmer_ZimmerId",
                        column: x => x.ZimmerId,
                        principalTable: "Zimmer",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bett_ZimmerId",
                table: "Bett",
                column: "ZimmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmer_XHotelsId",
                table: "Zimmer",
                column: "XHotelsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bett");

            migrationBuilder.DropTable(
                name: "Zimmer");

            migrationBuilder.DropTable(
                name: "Hotel");
        }
    }
}
