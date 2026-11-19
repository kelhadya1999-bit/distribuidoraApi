using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DistribuidoraApi.Migrations
{
    /// <inheritdoc />
    public partial class MigracionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoCategoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                    table.UniqueConstraint("AK_Categorias_CodigoCategoria", x => x.CodigoCategoria);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoCliente = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NifCif = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoProveedor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NifCif = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    RazonSocial = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnidadesMedida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoMedida = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadesMedida", x => x.Id);
                    table.UniqueConstraint("AK_UnidadesMedida_CodigoMedida", x => x.CodigoMedida);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodigoProducto = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrecioVenta = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PrecioCompra = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockActual = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    StockMinimo = table.Column<decimal>(type: "decimal(18,3)", precision: 18, scale: 3, nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    CodigoCategoria = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CodigoMedida = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CodigoCategoria",
                        column: x => x.CodigoCategoria,
                        principalTable: "Categorias",
                        principalColumn: "CodigoCategoria",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Productos_UnidadesMedida_CodigoMedida",
                        column: x => x.CodigoMedida,
                        principalTable: "UnidadesMedida",
                        principalColumn: "CodigoMedida",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "CodigoCategoria", "Nombre" },
                values: new object[] { new Guid("5c2e7a10-9d4b-4f61-8b3a-0e1f2a3b4c01"), "GEN", "General" });

            migrationBuilder.InsertData(
                table: "UnidadesMedida",
                columns: new[] { "Id", "CodigoMedida", "Nombre" },
                values: new object[,]
                {
                    { new Guid("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e01"), "UD", "Unidad" },
                    { new Guid("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e02"), "KG", "Kilogramo" },
                    { new Guid("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e03"), "L", "Litro" },
                    { new Guid("8a1f0c52-3b6e-4d2a-9f1e-1a2b3c4d5e04"), "CAJA", "Caja" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_CodigoCliente",
                table: "Clientes",
                column: "CodigoCliente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoCategoria",
                table: "Productos",
                column: "CodigoCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoMedida",
                table: "Productos",
                column: "CodigoMedida");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CodigoProducto",
                table: "Productos",
                column: "CodigoProducto",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_CodigoProveedor",
                table: "Proveedores",
                column: "CodigoProveedor",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "UnidadesMedida");
        }
    }
}
