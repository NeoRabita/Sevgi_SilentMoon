using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMoon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TranslationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TranslationId",
                table: "Topics",
                type: "NVARCHAR2(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Translations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Translation_AZ = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Translation_RU = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Translation_EN = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    RelationalId = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Slug = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Type = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IconUrl = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TranslationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Translations_TranslationId",
                        column: x => x.TranslationId,
                        principalTable: "Translations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Subtitle = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Type = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CategoryId = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ImageUrl = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DurationSec = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    NarratorType = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TranslationId = table.Column<string>(type: "NVARCHAR2(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Translations_TranslationId",
                        column: x => x.TranslationId,
                        principalTable: "Translations",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9307), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9319), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9321), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9322), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9323), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreateDate", "TranslationId" },
                values: new object[] { new DateTime(2026, 8, 18, 11, 32, 26, 15, DateTimeKind.Local).AddTicks(9324), null });

            migrationBuilder.CreateIndex(
                name: "IX_Topics_TranslationId",
                table: "Topics",
                column: "TranslationId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_TranslationId",
                table: "Categories",
                column: "TranslationId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TranslationId",
                table: "Courses",
                column: "TranslationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Translations_TranslationId",
                table: "Topics",
                column: "TranslationId",
                principalTable: "Translations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Translations_TranslationId",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Topics_TranslationId",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "TranslationId",
                table: "Topics");

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3711));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3714));
        }
    }
}
