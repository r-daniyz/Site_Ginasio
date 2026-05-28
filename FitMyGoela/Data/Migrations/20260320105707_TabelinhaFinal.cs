using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMyGoela.Data.Migrations
{
    /// <inheritdoc />
    public partial class TabelinhaFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TiposDeTreinos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TiposDeTreinos");
        }
    }
}
