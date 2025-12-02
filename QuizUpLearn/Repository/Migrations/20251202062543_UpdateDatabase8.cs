using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints",
                column: "UserMistakeId",
                principalTable: "UserMistakes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_UserWeakPoints_UserMistakes_UserMistakeId",
                table: "UserWeakPoints",
                column: "UserMistakeId",
                principalTable: "UserMistakes",
                principalColumn: "Id");
        }
    }
}
