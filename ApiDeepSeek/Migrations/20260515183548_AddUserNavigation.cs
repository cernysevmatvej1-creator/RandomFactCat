using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiDeepSeek.Migrations
{
    /// <inheritdoc />
    public partial class AddUserNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserPermissions_Users_UserId1",
                table: "UserPermissions");

            migrationBuilder.DropIndex(
                name: "IX_UserPermissions_UserId1",
                table: "UserPermissions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserPermissions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "UserPermissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserId1",
                table: "UserPermissions",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserPermissions_Users_UserId1",
                table: "UserPermissions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
