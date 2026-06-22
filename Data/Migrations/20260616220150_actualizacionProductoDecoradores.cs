        using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionProductoDecoradores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear la tabla tbCategoria
            migrationBuilder.CreateTable(
                name: "tbCategoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbCategoria", x => x.Id);
                });

            // Insertar un registro inicial en tbCategoria con Id = 0
            migrationBuilder.InsertData(
                table: "tbCategoria",
                columns: new[] { "Id", "Nombre" },
                values: new object[] { 0, "Sin Categoría" });

            // Agregar la columna CategoriaId a tbProducto
            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "tbProducto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Crear el índice y la clave foránea
            migrationBuilder.CreateIndex(
                name: "IX_tbProducto_CategoriaId",
                table: "tbProducto",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_tbProducto_tbCategoria_CategoriaId",
                table: "tbProducto",
                column: "CategoriaId",
                principalTable: "tbCategoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbProducto_tbCategoria_CategoriaId",
                table: "tbProducto");

            migrationBuilder.DropIndex(
                name: "IX_tbProducto_CategoriaId",
                table: "tbProducto");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "tbProducto");

            migrationBuilder.DropTable(
                name: "tbCategoria");
        }
    }
}
