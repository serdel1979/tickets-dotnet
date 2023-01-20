using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tickets.Migrations
{
    public partial class solicitudesestados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdSolicitud",
                table: "Estados",
                newName: "SolicitudId");

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Usuario = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Equipo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EstadoActual = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Estados_SolicitudId",
                table: "Estados",
                column: "SolicitudId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estados_Solicitudes_SolicitudId",
                table: "Estados",
                column: "SolicitudId",
                principalTable: "Solicitudes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estados_Solicitudes_SolicitudId",
                table: "Estados");

            migrationBuilder.DropTable(
                name: "Solicitudes");

            migrationBuilder.DropIndex(
                name: "IX_Estados_SolicitudId",
                table: "Estados");

            migrationBuilder.RenameColumn(
                name: "SolicitudId",
                table: "Estados",
                newName: "IdSolicitud");
        }
    }
}
