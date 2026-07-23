using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SilentMoon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TopicAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "RefreshTokens",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ApplicationUsers",
                type: "TIMESTAMP(7)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Slug = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Title = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    IconKey = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ColorHex = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTopics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    UserId = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    TopicId = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    TopicId1 = table.Column<int>(type: "NUMBER(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTopics_ApplicationUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTopics_Topics_TopicId1",
                        column: x => x.TopicId1,
                        principalTable: "Topics",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "ColorHex", "CreateDate", "IconKey", "Slug", "Title" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1854), null, "Sleep", "Sleepy" },
                    { 2, null, new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1871), null, "Stress", "Stressed" },
                    { 3, null, new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1873), null, "Anxiety", "Anxiety" },
                    { 4, null, new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1874), null, "Meditation", "Meditational" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_TopicId1",
                table: "UserTopics",
                column: "TopicId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTopics");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ApplicationUsers");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "ApplicationUsers",
                type: "NVARCHAR2(2000)",
                nullable: true);
        }
    }
}
