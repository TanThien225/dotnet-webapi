using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPi_App.Migrations
{
    /// <inheritdoc />
    public partial class AddTblCategory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Category_CategoryId1",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_CategoryId1",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Category");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Category",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryId1",
                table: "Category",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Category_CategoryId1",
                table: "Category",
                column: "CategoryId1",
                principalTable: "Category",
                principalColumn: "CategoryId");
        }
    }
}
