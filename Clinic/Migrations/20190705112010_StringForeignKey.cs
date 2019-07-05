using Microsoft.EntityFrameworkCore.Migrations;

namespace Clinic.Migrations
{
    public partial class StringForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Patient",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Patient",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Doctor",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Admin",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Admin");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Doctor",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
