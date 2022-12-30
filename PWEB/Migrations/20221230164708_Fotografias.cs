using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class Fotografias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografia_Recolhas_RecolhaVeiculoId",
                table: "Fotografia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotografia",
                table: "Fotografia");

            migrationBuilder.RenameTable(
                name: "Fotografia",
                newName: "Fotografias");

            migrationBuilder.RenameIndex(
                name: "IX_Fotografia_RecolhaVeiculoId",
                table: "Fotografias",
                newName: "IX_Fotografias_RecolhaVeiculoId");

            migrationBuilder.AlterColumn<int>(
                name: "RecolhaVeiculoId",
                table: "Fotografias",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotografias",
                table: "Fotografias",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografias_Recolhas_RecolhaVeiculoId",
                table: "Fotografias",
                column: "RecolhaVeiculoId",
                principalTable: "Recolhas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fotografias_Recolhas_RecolhaVeiculoId",
                table: "Fotografias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fotografias",
                table: "Fotografias");

            migrationBuilder.RenameTable(
                name: "Fotografias",
                newName: "Fotografia");

            migrationBuilder.RenameIndex(
                name: "IX_Fotografias_RecolhaVeiculoId",
                table: "Fotografia",
                newName: "IX_Fotografia_RecolhaVeiculoId");

            migrationBuilder.AlterColumn<int>(
                name: "RecolhaVeiculoId",
                table: "Fotografia",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fotografia",
                table: "Fotografia",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fotografia_Recolhas_RecolhaVeiculoId",
                table: "Fotografia",
                column: "RecolhaVeiculoId",
                principalTable: "Recolhas",
                principalColumn: "Id");
        }
    }
}
