using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class updaterelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_UserId",
                table: "SeekerProfiles");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Jobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_UserId",
                table: "SeekerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_UserId",
                table: "SeekerProfiles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
