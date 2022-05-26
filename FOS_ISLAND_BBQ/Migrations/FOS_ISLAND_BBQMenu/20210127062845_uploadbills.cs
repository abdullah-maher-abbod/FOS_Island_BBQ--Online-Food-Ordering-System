using Microsoft.EntityFrameworkCore.Migrations;

namespace FOS_ISLAND_BBQ.Migrations.FOS_ISLAND_BBQMenu
{
    public partial class uploadbills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadBills",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    imageURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadBills", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadBills");
        }
    }
}
