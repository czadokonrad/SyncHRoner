using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyncHRoner.Domain.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Profiles");

            migrationBuilder.CreateTable(
                name: "Country",
                schema: "Profiles",
                columns: table => new
                {
                    CountryId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                schema: "Profiles",
                columns: table => new
                {
                    GenderId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                schema: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(maxLength: 50, nullable: true),
                    LastName = table.Column<string>(maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: true),
                    GenderId = table.Column<long>(type: "bigint", nullable: false),
                    Phone = table.Column<string>(maxLength: 11, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    Rating = table.Column<double>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profile_Gender_GenderId",
                        column: x => x.GenderId,
                        principalSchema: "Profiles",
                        principalTable: "Gender",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileEmail",
                schema: "Profiles",
                columns: table => new
                {
                    ProfileEmailId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileEmail", x => x.ProfileEmailId);
                    table.ForeignKey(
                        name: "FK_ProfileEmail_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Profiles",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileNationality",
                schema: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileNationality", x => new { x.ProfileId, x.CountryId });
                    table.ForeignKey(
                        name: "FK_ProfileNationality_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "Profiles",
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileNationality_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Profiles",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfilePhone",
                schema: "Profiles",
                columns: table => new
                {
                    ProfilePhoneId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(maxLength: 11, nullable: false),
                    ProfileId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePhone", x => x.ProfilePhoneId);
                    table.ForeignKey(
                        name: "FK_ProfilePhone_Profile_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Profiles",
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "AK_Profile_Email",
                schema: "Profiles",
                table: "Profile",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_GenderId",
                schema: "Profiles",
                table: "Profile",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "AK_Profile_Phone",
                schema: "Profiles",
                table: "Profile",
                column: "Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileEmail_ProfileId",
                schema: "Profiles",
                table: "ProfileEmail",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileNationality_CountryId",
                schema: "Profiles",
                table: "ProfileNationality",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilePhone_ProfileId",
                schema: "Profiles",
                table: "ProfilePhone",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileEmail",
                schema: "Profiles");

            migrationBuilder.DropTable(
                name: "ProfileNationality",
                schema: "Profiles");

            migrationBuilder.DropTable(
                name: "ProfilePhone",
                schema: "Profiles");

            migrationBuilder.DropTable(
                name: "Country",
                schema: "Profiles");

            migrationBuilder.DropTable(
                name: "Profile",
                schema: "Profiles");

            migrationBuilder.DropTable(
                name: "Gender",
                schema: "Profiles");
        }
    }
}
