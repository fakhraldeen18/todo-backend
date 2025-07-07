using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updateUserprjectprimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects");

            migrationBuilder.DropIndex(
                name: "ix_user_projects_user_id",
                table: "user_projects");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_projects");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects",
                columns: new[] { "user_id", "project_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "user_projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_projects",
                table: "user_projects",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_user_projects_user_id",
                table: "user_projects",
                column: "user_id");
        }
    }
}
