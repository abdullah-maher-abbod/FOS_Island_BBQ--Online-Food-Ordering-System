using Microsoft.EntityFrameworkCore.Migrations;

namespace FOS_ISLAND_BBQ.Migrations.FOS_ISLAND_BBQMenu
{
    public partial class uploadads : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadAdveristment",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    imageURL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadAdveristment", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadAdveristment");
        }
    }
}
