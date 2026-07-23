using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SilentMoon.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TopicIdUpdatedInUserTopics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTopics_Topics_TopicId1",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopics_TopicId1",
                table: "UserTopics");

            migrationBuilder.DropColumn(
                name: "TopicId1",
                table: "UserTopics");

            migrationBuilder.AlterColumn<int>(
                name: "TopicId",
                table: "UserTopics",
                type: "NUMBER(10)",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "NVARCHAR2(2000)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 7, 23, 23, 31, 29, 836, DateTimeKind.Local).AddTicks(5632));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 7, 23, 23, 31, 29, 836, DateTimeKind.Local).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2026, 7, 23, 23, 31, 29, 836, DateTimeKind.Local).AddTicks(5647));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2026, 7, 23, 23, 31, 29, 836, DateTimeKind.Local).AddTicks(5649));

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics");

            migrationBuilder.AlterColumn<string>(
                name: "TopicId",
                table: "UserTopics",
                type: "NVARCHAR2(2000)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "NUMBER(10)");

            migrationBuilder.AddColumn<int>(
                name: "TopicId1",
                table: "UserTopics",
                type: "NUMBER(10)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1854));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1871));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1873));

            migrationBuilder.UpdateData(
                table: "Topics",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreateDate",
                value: new DateTime(2026, 7, 22, 22, 30, 17, 50, DateTimeKind.Local).AddTicks(1874));

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_TopicId1",
                table: "UserTopics",
                column: "TopicId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopics_Topics_TopicId1",
                table: "UserTopics",
                column: "TopicId1",
                principalTable: "Topics",
                principalColumn: "Id");
        }
    }
}
