using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCityComplaint.Migrations
{
    /// <inheritdoc />
    public partial class AddComplaintUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Complaints",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Complaints");
        }
    }
}
