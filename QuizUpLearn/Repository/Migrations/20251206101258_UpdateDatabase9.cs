using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserMistakes_UserWeakPointId",
                table: "UserMistakes",
                column: "UserWeakPointId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMistakes_UserWeakPoints_UserWeakPointId",
                table: "UserMistakes",
                column: "UserWeakPointId",
                principalTable: "UserWeakPoints",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserMistakes_UserWeakPoints_UserWeakPointId",
                table: "UserMistakes");

            migrationBuilder.DropIndex(
                name: "IX_UserMistakes_UserWeakPointId",
                table: "UserMistakes");

            migrationBuilder.AddColumn<Guid>(
                name: "UserMistakeId",
                table: "UserWeakPoints",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
