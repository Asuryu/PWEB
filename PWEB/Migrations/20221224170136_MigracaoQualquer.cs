using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class MigracaoQualquer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_AspNetUsers_ApplicationUserId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Veiculo_VeiculoId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculo_Empresa_EmpresaId",
                table: "Veiculo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Veiculo",
                table: "Veiculo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa");

            migrationBuilder.RenameTable(
                name: "Veiculo",
                newName: "Veiculos");

            migrationBuilder.RenameTable(
                name: "Reserva",
                newName: "Reservas");

            migrationBuilder.RenameTable(
                name: "Empresa",
                newName: "Empresas");

            migrationBuilder.RenameIndex(
                name: "IX_Veiculo_EmpresaId",
                table: "Veiculos",
                newName: "IX_Veiculos_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_VeiculoId",
                table: "Reservas",
                newName: "IX_Reservas_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_Reserva_ApplicationUserId",
                table: "Reservas",
                newName: "IX_Reservas_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Veiculos",
                table: "Veiculos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Empresas",
                table: "Empresas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId1",
                table: "AspNetUsers",
                column: "EmpresaId1",
                principalTable: "Empresas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_AspNetUsers_ApplicationUserId",
                table: "Reservas",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservas_Veiculos_VeiculoId",
                table: "Reservas",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "Id");

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
                name: "FK_AspNetUsers_Empresas_EmpresaId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Empresas_EmpresaId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_AspNetUsers_ApplicationUserId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservas_Veiculos_VeiculoId",
                table: "Reservas");

            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_Empresas_EmpresaId",
                table: "Veiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Veiculos",
                table: "Veiculos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservas",
                table: "Reservas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Empresas",
                table: "Empresas");

            migrationBuilder.RenameTable(
                name: "Veiculos",
                newName: "Veiculo");

            migrationBuilder.RenameTable(
                name: "Reservas",
                newName: "Reserva");

            migrationBuilder.RenameTable(
                name: "Empresas",
                newName: "Empresa");

            migrationBuilder.RenameIndex(
                name: "IX_Veiculos_EmpresaId",
                table: "Veiculo",
                newName: "IX_Veiculo_EmpresaId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_VeiculoId",
                table: "Reserva",
                newName: "IX_Reserva_VeiculoId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservas_ApplicationUserId",
                table: "Reserva",
                newName: "IX_Reserva_ApplicationUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Veiculo",
                table: "Veiculo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reserva",
                table: "Reserva",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Empresa",
                table: "Empresa",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId",
                table: "AspNetUsers",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Empresa_EmpresaId1",
                table: "AspNetUsers",
                column: "EmpresaId1",
                principalTable: "Empresa",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_AspNetUsers_ApplicationUserId",
                table: "Reserva",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Veiculo_VeiculoId",
                table: "Reserva",
                column: "VeiculoId",
                principalTable: "Veiculo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculo_Empresa_EmpresaId",
                table: "Veiculo",
                column: "EmpresaId",
                principalTable: "Empresa",
                principalColumn: "Id");
        }
    }
}
