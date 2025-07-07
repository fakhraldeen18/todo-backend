using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updateuserskillskey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills");

            migrationBuilder.DropIndex(
                name: "ix_user_skills_user_id",
                table: "user_skills");

            migrationBuilder.DropColumn(
                name: "id",
                table: "user_skills");

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills",
                columns: new[] { "user_id", "skill_id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "user_skills",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "pk_user_skills",
                table: "user_skills",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "ix_user_skills_user_id",
                table: "user_skills",
                column: "user_id");
        }
    }
}
