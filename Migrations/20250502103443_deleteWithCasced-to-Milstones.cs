using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteWithCascedtoMilstones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_milestones_milestone_id",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_milestones_milestone_id",
                table: "tasks",
                column: "milestone_id",
                principalTable: "milestones",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_tasks_milestones_milestone_id",
                table: "tasks");

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_milestones_milestone_id",
                table: "tasks",
                column: "milestone_id",
                principalTable: "milestones",
                principalColumn: "id");
        }
    }
}
