using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class ConsultationsOverhauling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Consultation_DoctorId",
                table: "Consultation",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Consultation_PatientId",
                table: "Consultation",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Doctor_DoctorId",
                table: "Consultation",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Consultation_Patient_PatientId",
                table: "Consultation",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Doctor_DoctorId",
                table: "Consultation");

            migrationBuilder.DropForeignKey(
                name: "FK_Consultation_Patient_PatientId",
                table: "Consultation");

            migrationBuilder.DropIndex(
                name: "IX_Consultation_DoctorId",
                table: "Consultation");

            migrationBuilder.DropIndex(
                name: "IX_Consultation_PatientId",
                table: "Consultation");
        }
    }
}
