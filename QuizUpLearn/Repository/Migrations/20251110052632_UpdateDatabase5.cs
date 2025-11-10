using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeakPoints",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserAnswer",
                table: "UserMistakes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserWeakPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    QuizSetId = table.Column<Guid>(type: "uuid", nullable: true),
                    WeakPoint = table.Column<string>(type: "text", nullable: false),
                    Advice = table.Column<string>(type: "text", nullable: true),
                    IsDone = table.Column<bool>(type: "boolean", nullable: false),
                    CompleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserWeakPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserWeakPoints_QuizSets_QuizSetId",
                        column: x => x.QuizSetId,
                        principalTable: "QuizSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserWeakPoints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserWeakPoints_QuizSetId",
                table: "UserWeakPoints",
                column: "QuizSetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserWeakPoints_UserId",
                table: "UserWeakPoints",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "UserAnswer",
                table: "UserMistakes");

            migrationBuilder.AddColumn<List<string>>(
                name: "WeakPoints",
                table: "Users",
                type: "text[]",
                nullable: true);
        }
    }
}
