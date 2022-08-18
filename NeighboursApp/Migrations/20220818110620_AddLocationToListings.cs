using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neighbours.Migrations
{
    public partial class AddLocationToListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Listings",
                newName: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Item_Service",
                table: "Listings",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Item_Service",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Listings",
                newName: "Name");
        }
    }
}
