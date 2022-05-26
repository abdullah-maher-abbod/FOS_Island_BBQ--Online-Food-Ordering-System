using Microsoft.EntityFrameworkCore.Migrations;

namespace FOS_ISLAND_BBQ.Migrations.FOS_ISLAND_BBQMenu
{
    public partial class UploadBls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "UploadBills",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "UploadBills");
        }
    }
}
