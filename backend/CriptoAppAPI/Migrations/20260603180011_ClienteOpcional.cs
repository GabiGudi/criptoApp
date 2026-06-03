using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriptoAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class ClienteOpcional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_cliente_id",
                table: "Transacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_cliente_id",
                table: "Transacciones",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transacciones_Clientes_cliente_id",
                table: "Transacciones");

            migrationBuilder.AddForeignKey(
                name: "FK_Transacciones_Clientes_cliente_id",
                table: "Transacciones",
                column: "cliente_id",
                principalTable: "Clientes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
