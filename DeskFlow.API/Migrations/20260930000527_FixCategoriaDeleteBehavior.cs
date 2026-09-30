using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeskFlow.API.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoriaDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados");

            migrationBuilder.AddForeignKey(
                name: "FK_Chamados_Categorias_CategoriaId",
                table: "Chamados",
                column: "CategoriaId",
                principalTable: "Categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
