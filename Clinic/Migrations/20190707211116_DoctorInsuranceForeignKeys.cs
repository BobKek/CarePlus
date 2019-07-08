using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class DoctorInsuranceForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InsuranceCompany",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCompany_UserId",
                table: "InsuranceCompany",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_AspNetUsers_UserId",
                table: "Doctor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_UserId",
                table: "InsuranceCompany",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_AspNetUsers_UserId",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_InsuranceCompany_AspNetUsers_UserId",
                table: "InsuranceCompany");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCompany_UserId",
                table: "InsuranceCompany");

            migrationBuilder.DropIndex(
                name: "IX_Doctor_UserId",
                table: "Doctor");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "InsuranceCompany",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
