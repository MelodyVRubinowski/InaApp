using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace inaApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AtributoEstadoACategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "tbCategoria",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "tbCategoria");
        }
    }
}
