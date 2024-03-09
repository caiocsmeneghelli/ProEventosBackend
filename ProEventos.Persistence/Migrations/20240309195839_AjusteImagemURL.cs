using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProEventos.Persistence.Migrations
{
    public partial class AjusteImagemURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemPerfil",
                table: "AspNetUsers",
                newName: "ImagemURL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemURL",
                table: "AspNetUsers",
                newName: "ImagemPerfil");
        }
    }
}
