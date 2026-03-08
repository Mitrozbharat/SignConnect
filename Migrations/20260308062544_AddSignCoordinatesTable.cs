using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartVideoCallApp.Migrations
{
    /// <inheritdoc />
    public partial class AddSignCoordinatesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignCoordinates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Label = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TimeId = table.Column<long>(type: "bigint", nullable: false),
                    CoordinatesJson = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignCoordinates", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SignCoordinates_CreatedAtUtc",
                table: "SignCoordinates",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_SignCoordinates_Label",
                table: "SignCoordinates",
                column: "Label");

            migrationBuilder.CreateIndex(
                name: "IX_SignCoordinates_TimeId",
                table: "SignCoordinates",
                column: "TimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignCoordinates");
        }
    }
}
