using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Regenerated_Poll4674 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Polls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Polls_UserId",
                table: "Polls",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Polls_AbpUsers_UserId",
                table: "Polls",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Polls_AbpUsers_UserId",
                table: "Polls");

            migrationBuilder.DropIndex(
                name: "IX_Polls_UserId",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Polls");
        }
    }
}
