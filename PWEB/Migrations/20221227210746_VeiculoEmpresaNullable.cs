using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class VeiculoEmpresaNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_Empresas_EmpresaId",
                table: "Veiculos");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Veiculos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_Empresas_EmpresaId",
                table: "Veiculos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_Empresas_EmpresaId",
                table: "Veiculos");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "Veiculos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_Empresas_EmpresaId",
                table: "Veiculos",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
