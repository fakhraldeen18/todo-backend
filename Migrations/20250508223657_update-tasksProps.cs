using Harkh_backend.src.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatetasksProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "tasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(Status),
                oldType: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Status>(
                name: "status",
                table: "tasks",
                type: "status",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
