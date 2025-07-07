using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updateusersentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "porfile_image",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "profile_image",
                table: "users",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "profile_image",
                table: "users");

            migrationBuilder.AddColumn<string>(
                name: "porfile_image",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
