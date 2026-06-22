using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrudCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido1",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "Apellido2",
                table: "tbCliente");

            migrationBuilder.RenameColumn(
                name: "FechaNac",
                table: "tbCliente",
                newName: "FechaCreacion");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "tbCliente",
                newName: "Activo");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "tbCliente",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "CorreoElectronico",
                table: "tbCliente",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroIdentificacion",
                table: "tbCliente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PrimerApellido",
                table: "tbCliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SegundoApellido",
                table: "tbCliente",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "tbCliente",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoIdentificacion",
                table: "tbCliente",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_tbCliente_TipoIdentificacion_NumeroIdentificacion",
                table: "tbCliente",
                columns: new[] { "TipoIdentificacion", "NumeroIdentificacion" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbCliente_TipoIdentificacion_NumeroIdentificacion",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "CorreoElectronico",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "NumeroIdentificacion",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "PrimerApellido",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "SegundoApellido",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "tbCliente");

            migrationBuilder.DropColumn(
                name: "TipoIdentificacion",
                table: "tbCliente");

            migrationBuilder.RenameColumn(
                name: "FechaCreacion",
                table: "tbCliente",
                newName: "FechaNac");

            migrationBuilder.RenameColumn(
                name: "Activo",
                table: "tbCliente",
                newName: "Estado");

            migrationBuilder.AlterColumn<string>(
                name: "Nombre",
                table: "tbCliente",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Apellido1",
                table: "tbCliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Apellido2",
                table: "tbCliente",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
