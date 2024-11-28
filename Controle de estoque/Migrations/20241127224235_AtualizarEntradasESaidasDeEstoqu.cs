using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Controle_de_estoque.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarEntradasESaidasDeEstoqu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FornecedorId1",
                table: "EntradasEstoque",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntradasEstoque_FornecedorId1",
                table: "EntradasEstoque",
                column: "FornecedorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_EntradasEstoque_Fornecedores_FornecedorId1",
                table: "EntradasEstoque",
                column: "FornecedorId1",
                principalTable: "Fornecedores",
                principalColumn: "FornecedorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntradasEstoque_Fornecedores_FornecedorId1",
                table: "EntradasEstoque");

            migrationBuilder.DropIndex(
                name: "IX_EntradasEstoque_FornecedorId1",
                table: "EntradasEstoque");

            migrationBuilder.DropColumn(
                name: "FornecedorId1",
                table: "EntradasEstoque");
        }
    }
}
