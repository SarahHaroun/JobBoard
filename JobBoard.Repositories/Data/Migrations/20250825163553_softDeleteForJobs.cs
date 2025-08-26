using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class softDeleteForJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_SeekerProfiles_ApplicantId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EmployerProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_SeekerProfiles_ApplicantId",
                table: "Applications",
                column: "ApplicantId",
                principalTable: "SeekerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_SeekerProfiles_ApplicantId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EmployerProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Jobs_JobId",
                table: "Applications",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_SeekerProfiles_ApplicantId",
                table: "Applications",
                column: "ApplicantId",
                principalTable: "SeekerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
