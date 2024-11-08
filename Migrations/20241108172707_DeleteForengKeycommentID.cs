using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotNetShop.Migrations
{
    /// <inheritdoc />
    public partial class DeleteForengKeycommentID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Comments_CommentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CommentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CommentId",
                table: "Products",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Comments_CommentId",
                table: "Products",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
