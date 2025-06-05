using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class BettfixReserwierung : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AllBett",
                table: "Reservierung",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllBett",
                table: "Reservierung");
        }
    }
}
