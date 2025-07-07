using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updateuserprojectsAndSkillsEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_user_skills_user_id",
                table: "user_skills",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_projects_user_id",
                table: "user_projects",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills");

            migrationBuilder.DropIndex(
                name: "ix_user_skills_user_id",
                table: "user_skills");

            migrationBuilder.DropPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects");

            migrationBuilder.DropIndex(
                name: "ix_user_projects_user_id",
                table: "user_projects");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills",
                columns: new[] { "user_id", "skill_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects",
                columns: new[] { "user_id", "project_id" });
        }
    }
}
