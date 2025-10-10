using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NEWCODES.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalIngres",
                table: "Localidades",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Conteo",
                table: "Codigos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalIngres",
                table: "Localidades");

            migrationBuilder.DropColumn(
                name: "Conteo",
                table: "Codigos");
        }
    }
}
