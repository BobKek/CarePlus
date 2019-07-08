using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class AdminForeignKeyEdit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Admin",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admin_UserId",
                table: "Admin",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin");

            migrationBuilder.DropIndex(
                name: "IX_Admin_UserId",
                table: "Admin");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Admin",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
