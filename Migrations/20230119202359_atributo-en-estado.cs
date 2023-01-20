using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tickets.Migrations
{
    public partial class atributoenestado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripcion",
                table: "Estados",
                newName: "EstadoActual");

            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "Estados",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "Estados");

            migrationBuilder.RenameColumn(
                name: "EstadoActual",
                table: "Estados",
                newName: "Descripcion");
        }
    }
}
