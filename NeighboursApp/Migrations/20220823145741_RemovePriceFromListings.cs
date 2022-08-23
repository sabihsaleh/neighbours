using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace neighbours.Migrations
{
    public partial class RemovePriceFromListings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Listings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Listings",
                type: "text",
                nullable: true);
        }
    }
}
