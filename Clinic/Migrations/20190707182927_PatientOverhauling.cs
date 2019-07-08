using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class PatientOverhauling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Patient",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patient_InsuranceId",
                table: "Patient",
                column: "InsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_UserId",
                table: "Patient",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_InsuranceCompany_InsuranceId",
                table: "Patient",
                column: "InsuranceId",
                principalTable: "InsuranceCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_AspNetUsers_UserId",
                table: "Patient",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patient_InsuranceCompany_InsuranceId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_AspNetUsers_UserId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_InsuranceId",
                table: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Patient_UserId",
                table: "Patient");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Patient",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
