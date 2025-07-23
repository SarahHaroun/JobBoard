using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_EmployerProfiles_employerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SeekerProfiles_seekerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_employerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_seekerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "employerProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "seekerProfileId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "employerProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "seekerProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_employerProfileId",
                table: "AspNetUsers",
                column: "employerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_seekerProfileId",
                table: "AspNetUsers",
                column: "seekerProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_EmployerProfiles_employerProfileId",
                table: "AspNetUsers",
                column: "employerProfileId",
                principalTable: "EmployerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SeekerProfiles_seekerProfileId",
                table: "AspNetUsers",
                column: "seekerProfileId",
                principalTable: "SeekerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
