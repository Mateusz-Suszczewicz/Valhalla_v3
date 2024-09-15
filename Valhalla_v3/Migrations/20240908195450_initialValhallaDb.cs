using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valhalla_v3.Migrations
{
    /// <inheritdoc />
    public partial class initialValhallaDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Operators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Projects_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Term = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tasks_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: true),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComments_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskComments_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskComments_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OperatorCreateId",
                table: "Projects",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OperatorModifyId",
                table: "Projects",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_OperatorCreateId",
                table: "TaskComments",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_OperatorModifyId",
                table: "TaskComments",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComments",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OperatorCreateId",
                table: "Tasks",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OperatorModifyId",
                table: "Tasks",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId",
                table: "Tasks",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Operators");
        }
    }
}
