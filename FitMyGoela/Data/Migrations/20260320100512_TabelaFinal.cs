using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMyGoela.Data.Migrations
{
    /// <inheritdoc />
    public partial class TabelaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exercicios",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercicios");
        }
    }
}
