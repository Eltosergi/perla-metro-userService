using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace perla_metro_user.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeleteHistorials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdminId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteHistorials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeleteHistorials_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeleteHistorials_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeleteHistorials_AdminId",
                table: "DeleteHistorials",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_DeleteHistorials_UserId",
                table: "DeleteHistorials",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeleteHistorials");
        }
    }
}
