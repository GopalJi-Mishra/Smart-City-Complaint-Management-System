using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartCityComplaint.Migrations
{
    /// <inheritdoc />
    public partial class AddAIAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AIAnalysis",
                table: "Complaints",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AIAnalysis",
                table: "Complaints");
        }
    }
}
