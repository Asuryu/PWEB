using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PWEB.Migrations
{
    public partial class Fixe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId1",
                table: "Gestores");

            migrationBuilder.DropIndex(
                name: "IX_Gestores_ApplicationUserId1",
                table: "Gestores");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Gestores");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Gestores",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Gestores_ApplicationUserId",
                table: "Gestores",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId",
                table: "Gestores",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId",
                table: "Gestores");

            migrationBuilder.DropIndex(
                name: "IX_Gestores_ApplicationUserId",
                table: "Gestores");

            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Gestores",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Gestores",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gestores_ApplicationUserId1",
                table: "Gestores",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gestores_AspNetUsers_ApplicationUserId1",
                table: "Gestores",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
