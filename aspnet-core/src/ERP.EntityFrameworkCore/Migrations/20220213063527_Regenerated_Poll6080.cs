using Microsoft.EntityFrameworkCore.Migrations;

namespace ERP.Migrations
{
    public partial class Regenerated_Poll6080 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "count1",
                table: "Polls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "count2",
                table: "Polls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "count3",
                table: "Polls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "count4",
                table: "Polls",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "count1",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "count2",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "count3",
                table: "Polls");

            migrationBuilder.DropColumn(
                name: "count4",
                table: "Polls");
        }
    }
}
