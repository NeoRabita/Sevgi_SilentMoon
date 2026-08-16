using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SilentMoon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TopicDatasUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3699), "betterSleep.png", "Better Sleep" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3709), "reduceStress.png", "Reduce Stressed" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3711), "reduceAnxiety.png", "Reduce Anxiety" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "IconKey", "Slug", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3712), "increaseHappiness.png", "Happiness", "Increase Happiness" });

            migrationBuilder.InsertData(
                table: "Topics",
                columns: new[] { "Id", "ColorHex", "CreateDate", "IconKey", "IsDeleted", "Slug", "Title" },
                values: new object[,]
                {
                    { 5, null, new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3713), "improvePerformance.png", false, "Performance", "Improve Performance" },
                    { 6, null, new DateTime(2026, 8, 16, 14, 50, 8, 494, DateTimeKind.Local).AddTicks(3714), "personalGrowth.png", false, "Growth", "Personal Growth" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(225), null, "Sleepy" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(291), "reduceStress", "Stressed" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreateDate", "IconKey", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(293), null, "Anxiety" });

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreateDate", "IconKey", "Slug", "Title" },
                values: new object[] { new DateTime(2026, 8, 16, 13, 28, 41, 357, DateTimeKind.Local).AddTicks(295), null, "Meditation", "Meditational" });
        }
    }
}
