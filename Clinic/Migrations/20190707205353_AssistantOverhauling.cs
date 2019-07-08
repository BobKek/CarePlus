using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class AssistantOverhauling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistant",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assistant_DoctorId",
                table: "Assistant",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Assistant_UserId",
                table: "Assistant",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistant_Doctor_DoctorId",
                table: "Assistant",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Assistant_AspNetUsers_UserId",
                table: "Assistant",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistant_Doctor_DoctorId",
                table: "Assistant");

            migrationBuilder.DropForeignKey(
                name: "FK_Assistant_AspNetUsers_UserId",
                table: "Assistant");

            migrationBuilder.DropIndex(
                name: "IX_Assistant_DoctorId",
                table: "Assistant");

            migrationBuilder.DropIndex(
                name: "IX_Assistant_UserId",
                table: "Assistant");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Assistant",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
