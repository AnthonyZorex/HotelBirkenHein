using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motel_birkenhein.Migrations
{
    /// <inheritdoc />
    public partial class Updatedb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Zimmernummer",
                table: "Reservierung",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zimmernummer",
                table: "Reservierung");
        }
    }
}
