using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tickets.Migrations
{
    public partial class atributourlimagen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImagen",
                table: "Solicitudes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlImagen",
                table: "Equipos",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImagen",
                table: "Solicitudes");

            migrationBuilder.DropColumn(
                name: "UrlImagen",
                table: "Equipos");
        }
    }
}
