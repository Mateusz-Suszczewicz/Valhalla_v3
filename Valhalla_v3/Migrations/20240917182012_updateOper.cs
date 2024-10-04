using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valhalla_v3.Migrations
{
    /// <inheritdoc />
    public partial class updateOper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "TaskComments",
                newName: "JobId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                newName: "IX_TaskComments_JobId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeAdd",
                table: "Operators",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateTimeModify",
                table: "Operators",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_JobId",
                table: "TaskComments",
                column: "JobId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_JobId",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "DateTimeAdd",
                table: "Operators");

            migrationBuilder.DropColumn(
                name: "DateTimeModify",
                table: "Operators");

            migrationBuilder.RenameColumn(
                name: "JobId",
                table: "TaskComments",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_JobId",
                table: "TaskComments",
                newName: "IX_TaskComments_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
