using Microsoft.EntityFrameworkCore.Migrations;

namespace TAPPT.Web.Migrations
{
    public partial class TestNewMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdNumber",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalAddress",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Residence",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostalAddress",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Residence",
                table: "AspNetUsers");
        }
    }
}
