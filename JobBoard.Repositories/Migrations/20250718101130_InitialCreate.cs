using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployerProfiles_UserId",
                table: "EmployerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Skills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SeekerProfiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployerProfiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_UserId",
                table: "EmployerProfiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_AspNetUsers_UserId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_SeekerProfiles_AspNetUsers_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployerProfiles_UserId",
                table: "EmployerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "Skills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SeekerProfiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmployerProfiles",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_UserId",
                table: "EmployerProfiles",
                column: "UserId");

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
    }
}
