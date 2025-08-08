using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateJobsAndEmployersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeesNumber",
                table: "EmployerProfiles");

            migrationBuilder.RenameColumn(
                name: "Companymission",
                table: "EmployerProfiles",
                newName: "CompanyMission");

            migrationBuilder.RenameColumn(
                name: "Companylogo",
                table: "EmployerProfiles",
                newName: "EmployeeRange");

            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Responsabilities",
                table: "Jobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLocation",
                table: "EmployerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Responsabilities",
                table: "Jobs");

            migrationBuilder.RenameColumn(
                name: "CompanyMission",
                table: "EmployerProfiles",
                newName: "Companymission");

            migrationBuilder.RenameColumn(
                name: "EmployeeRange",
                table: "EmployerProfiles",
                newName: "Companylogo");

            migrationBuilder.AlterColumn<string>(
                name: "CompanyLocation",
                table: "EmployerProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "EmployeesNumber",
                table: "EmployerProfiles",
                type: "int",
                nullable: true);
        }
    }
}
