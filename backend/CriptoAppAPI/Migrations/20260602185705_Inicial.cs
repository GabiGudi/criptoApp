using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CriptoAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Transacciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    crypto_code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    crypto_amount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    money = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    datetime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cliente_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transacciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transacciones_Clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "Clientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transacciones_cliente_id",
                table: "Transacciones",
                column: "cliente_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transacciones");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
