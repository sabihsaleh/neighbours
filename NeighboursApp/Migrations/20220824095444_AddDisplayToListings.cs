using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neighbours.Migrations
{
    public partial class AddDisplayToListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Display",
                table: "Listings",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Display",
                table: "Listings");
        }
    }
}
