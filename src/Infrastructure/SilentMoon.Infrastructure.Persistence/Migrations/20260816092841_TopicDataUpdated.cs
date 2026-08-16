using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMoon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TopicDataUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(225));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "IconKey" },
                values: new object[] { new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(291), "reduceStress" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(295));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 7, 30, 17, 24, 18, 380, DateTimeKind.Local).AddTicks(5509));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "IconKey" },
                values: new object[] { new DateTime(2026, 7, 30, 17, 24, 18, 380, DateTimeKind.Local).AddTicks(5522), null });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2026, 7, 30, 17, 24, 18, 380, DateTimeKind.Local).AddTicks(5523));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2026, 7, 30, 17, 24, 18, 380, DateTimeKind.Local).AddTicks(5524));
        }
    }
}
