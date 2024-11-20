using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelInspiration.Api.Shared.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SuggestedFieldAddedToStop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Suggested",
                table: "Stops",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Itineraries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1560));

            migrationBuilder.UpdateData(
                table: "Itineraries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1726));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1736));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1740));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1744));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 17, 21, 46, 55, 275, DateTimeKind.Utc).AddTicks(1747));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suggested",
                table: "Stops");

            migrationBuilder.UpdateData(
                table: "Itineraries",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 632, DateTimeKind.Utc).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "Itineraries",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 632, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(34));

            migrationBuilder.UpdateData(
                table: "Stops",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedOn",
                value: new DateTime(2024, 11, 16, 11, 57, 1, 633, DateTimeKind.Utc).AddTicks(37));
        }
    }
}
