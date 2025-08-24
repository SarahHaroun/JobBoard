using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class changedeletebehaviorofjob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_EmployerProfiles_EmployerId",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_EmployerProfiles_EmployerId",
                table: "Jobs",
                column: "EmployerId",
                principalTable: "EmployerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_EmployerProfiles_EmployerId",
                table: "Jobs");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_EmployerProfiles_EmployerId",
                table: "Jobs",
                column: "EmployerId",
                principalTable: "EmployerProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
