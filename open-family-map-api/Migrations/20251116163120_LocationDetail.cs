using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace open_family_map.Migrations
{
    /// <inheritdoc />
    public partial class LocationDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail");

            migrationBuilder.RenameColumn(
                name: "UpdatedDate",
                table: "LocationDetail",
                newName: "LocationUpdateDate");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LocationDetail",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AltitudeMeters",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BearingAccuracyDegrees",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "BearingDegrees",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ElapsedRealtimeSinceBoot",
                table: "LocationDetail",
                type: "time(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorLevel",
                table: "LocationDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "HorizontalAccuracyMeters",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMock",
                table: "LocationDetail",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "LocationDetail",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "LocationDetail",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Platform",
                table: "LocationDetail",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RawProvider",
                table: "LocationDetail",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpeedAccuracyMetersPerSecond",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SpeedMetersPerSecond",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VerticalAccuracyMeters",
                table: "LocationDetail",
                type: "double",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationDetail_CreatedDate",
                table: "LocationDetail",
                column: "CreatedDate");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail");

            migrationBuilder.DropIndex(
                name: "IX_LocationDetail_CreatedDate",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "AltitudeMeters",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "BearingAccuracyDegrees",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "BearingDegrees",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "ElapsedRealtimeSinceBoot",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "FloorLevel",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "HorizontalAccuracyMeters",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "IsMock",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "Platform",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "RawProvider",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "SpeedAccuracyMetersPerSecond",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "SpeedMetersPerSecond",
                table: "LocationDetail");

            migrationBuilder.DropColumn(
                name: "VerticalAccuracyMeters",
                table: "LocationDetail");

            migrationBuilder.RenameColumn(
                name: "LocationUpdateDate",
                table: "LocationDetail",
                newName: "UpdatedDate");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "LocationDetail",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
