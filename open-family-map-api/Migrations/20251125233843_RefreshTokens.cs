using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace open_family_map.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationDetail",
                table: "LocationDetail");

            migrationBuilder.RenameTable(
                name: "LocationDetail",
                newName: "LocationDetails");

            migrationBuilder.RenameIndex(
                name: "IX_LocationDetail_UserId",
                table: "LocationDetails",
                newName: "IX_LocationDetails_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationDetail_CreatedDate",
                table: "LocationDetails",
                newName: "IX_LocationDetails_CreatedDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationDetails",
                table: "LocationDetails",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "varchar(255)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsRevoked = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsUsed = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Token);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDetails_Users_UserId",
                table: "LocationDetails",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationDetails_Users_UserId",
                table: "LocationDetails");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocationDetails",
                table: "LocationDetails");

            migrationBuilder.RenameTable(
                name: "LocationDetails",
                newName: "LocationDetail");

            migrationBuilder.RenameIndex(
                name: "IX_LocationDetails_UserId",
                table: "LocationDetail",
                newName: "IX_LocationDetail_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_LocationDetails_CreatedDate",
                table: "LocationDetail",
                newName: "IX_LocationDetail_CreatedDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocationDetail",
                table: "LocationDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationDetail_Users_UserId",
                table: "LocationDetail",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
