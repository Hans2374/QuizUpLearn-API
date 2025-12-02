using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWeakPoints_QuizSets_QuizSetId",
                table: "UserWeakPoints");

            migrationBuilder.DropIndex(
                name: "IX_UserWeakPoints_QuizSetId",
                table: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "CompleteAt",
                table: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "IsDone",
                table: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "QuizSetId",
                table: "UserWeakPoints");

            migrationBuilder.AddColumn<Guid>(
                name: "UserMistakeId",
                table: "UserWeakPoints",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserWeakPointId",
                table: "UserMistakes",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWeakPoints_UserMistakeId",
                table: "UserWeakPoints",
                column: "UserMistakeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints",
                column: "UserMistakeId",
                principalTable: "UserMistakes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints");

            migrationBuilder.DropIndex(
                name: "IX_UserWeakPoints_UserMistakeId",
                table: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "UserMistakeId",
                table: "UserWeakPoints");

            migrationBuilder.DropColumn(
                name: "UserWeakPointId",
                table: "UserMistakes");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompleteAt",
                table: "UserWeakPoints",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDone",
                table: "UserWeakPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "QuizSetId",
                table: "UserWeakPoints",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserWeakPoints_QuizSetId",
                table: "UserWeakPoints",
                column: "QuizSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWeakPoints_QuizSets_QuizSetId",
                table: "UserWeakPoints",
                column: "QuizSetId",
                principalTable: "QuizSets",
                principalColumn: "Id");
        }
    }
}
