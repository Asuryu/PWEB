using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class ReservaEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Reservas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_EmpresaId",
                table: "Reservas",
                column: "EmpresaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Empresas_EmpresaId",
                table: "Reservas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Empresas_EmpresaId",
                table: "Reservas");

            migrationBuilder.DropIndex(
                name: "IX_Reservas_EmpresaId",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Reservas");
        }
    }
}
