using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Hotel_Zimmer_reletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zimmer_Hotel_XHotelsId",
                table: "Zimmer");

            migrationBuilder.DropIndex(
                name: "IX_Zimmer_XHotelsId",
                table: "Zimmer");

            migrationBuilder.DropColumn(
                name: "XHotelsId",
                table: "Zimmer");

            migrationBuilder.CreateIndex(
                name: "IX_Zimmer_XHotelId",
                table: "Zimmer",
                column: "XHotelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zimmer_Hotel_XHotelId",
                table: "Zimmer",
                column: "XHotelId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zimmer_Hotel_XHotelId",
                table: "Zimmer");

            migrationBuilder.DropIndex(
                name: "IX_Zimmer_XHotelId",
                table: "Zimmer");

            migrationBuilder.AddColumn<Guid>(
                name: "XHotelsId",
                table: "Zimmer",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Zimmer_XHotelsId",
                table: "Zimmer",
                column: "XHotelsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zimmer_Hotel_XHotelsId",
                table: "Zimmer",
                column: "XHotelsId",
                principalTable: "Hotel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
