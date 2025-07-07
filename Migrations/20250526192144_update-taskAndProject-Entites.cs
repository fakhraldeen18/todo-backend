using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class updatetaskAndProjectEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projects_users_manager_id",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "manager_name",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "projects",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "ix_projects_manager_id",
                table: "projects",
                newName: "ix_projects_owner_id");

            migrationBuilder.AddColumn<Guid>(
                name: "assignee_to",
                table: "tasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "OwnerId",
                table: "projects",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "manager_id",
                table: "projects",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_users_owner_id",
                table: "projects",
                column: "OwnerId",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_projects_users_owner_id",
                table: "projects");

            migrationBuilder.DropColumn(
                name: "assignee_to",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "manager_id",
                table: "projects");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "projects",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "ix_projects_owner_id",
                table: "projects",
                newName: "ix_projects_manager_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "ManagerId",
                table: "projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "manager_name",
                table: "projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_projects_users_manager_id",
                table: "projects",
                column: "ManagerId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
