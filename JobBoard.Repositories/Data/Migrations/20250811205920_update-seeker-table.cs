using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobBoard.Repositories.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateseekertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "Experience_Level",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "SeekerProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "Skills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
                name: "Gender",
                table: "SeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "CV_Url",
                table: "SeekerProfiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SeekerProfiles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "SeekerProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SeekerProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "SeekerProfiles",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "SeekerProfiles",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "SeekerProfiles",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SeekerCertificates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CertificateName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeekerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeekerCertificates_SeekerProfiles_SeekerProfileId",
                        column: x => x.SeekerProfileId,
                        principalTable: "SeekerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeekerEducations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Major = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Faculty = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    University = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GPA = table.Column<double>(type: "float", nullable: true),
                    EducationLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeekerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerEducations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeekerEducations_SeekerProfiles_SeekerProfileId",
                        column: x => x.SeekerProfileId,
                        principalTable: "SeekerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeekerExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SeekerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeekerExperiences_SeekerProfiles_SeekerProfileId",
                        column: x => x.SeekerProfileId,
                        principalTable: "SeekerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "seekerInterests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InterestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeekerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seekerInterests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_seekerInterests_SeekerProfiles_SeekerProfileId",
                        column: x => x.SeekerProfileId,
                        principalTable: "SeekerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeekerTrainings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainingName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeekerProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeekerTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeekerTrainings_SeekerProfiles_SeekerProfileId",
                        column: x => x.SeekerProfileId,
                        principalTable: "SeekerProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeekerCertificates_SeekerProfileId",
                table: "SeekerCertificates",
                column: "SeekerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerEducations_SeekerProfileId",
                table: "SeekerEducations",
                column: "SeekerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerExperiences_SeekerProfileId",
                table: "SeekerExperiences",
                column: "SeekerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_seekerInterests_SeekerProfileId",
                table: "seekerInterests",
                column: "SeekerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerTrainings_SeekerProfileId",
                table: "SeekerTrainings",
                column: "SeekerProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeekerCertificates");

            migrationBuilder.DropTable(
                name: "SeekerEducations");

            migrationBuilder.DropTable(
                name: "SeekerExperiences");

            migrationBuilder.DropTable(
                name: "seekerInterests");

            migrationBuilder.DropTable(
                name: "SeekerTrainings");

            migrationBuilder.DropIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "SeekerProfiles");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "SeekerProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "SkillName",
                table: "Skills",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "SeekerProfiles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "SeekerProfiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CV_Url",
                table: "SeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SeekerProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Experience_Level",
                table: "SeekerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "SeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "SeekerProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SeekerProfiles_UserId",
                table: "SeekerProfiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
