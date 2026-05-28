using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitMyGoela.Data.Migrations
{
    /// <inheritdoc />
    public partial class Tabelas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercicios_TiposDeTreinos_TiposDeTreinoId",
                table: "Exercicios");

            migrationBuilder.AlterColumn<int>(
                name: "TiposDeTreinoId",
                table: "Exercicios",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Imagem",
                table: "Exercicios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercicios_TiposDeTreinos_TiposDeTreinoId",
                table: "Exercicios",
                column: "TiposDeTreinoId",
                principalTable: "TiposDeTreinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercicios_TiposDeTreinos_TiposDeTreinoId",
                table: "Exercicios");

            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Exercicios");

            migrationBuilder.AlterColumn<int>(
                name: "TiposDeTreinoId",
                table: "Exercicios",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercicios_TiposDeTreinos_TiposDeTreinoId",
                table: "Exercicios",
                column: "TiposDeTreinoId",
                principalTable: "TiposDeTreinos",
                principalColumn: "Id");
        }
    }
}
