using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class VeiculoAtivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Veiculos",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Veiculos");
        }
    }
}
