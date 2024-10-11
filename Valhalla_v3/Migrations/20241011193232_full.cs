using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valhalla_v3.Migrations
{
    /// <inheritdoc />
    public partial class full : Migration
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
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EngineCC = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    VIN = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Car_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Car_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GasStation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GasStation_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GasStation_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Mechanic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StreetNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mechanic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mechanic_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Mechanic_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
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
                name: "CarHistoryFuel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    GasStationId = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CostPerLitr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHistoryFuel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarHistoryFuel_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarHistoryFuel_GasStation_GasStationId",
                        column: x => x.GasStationId,
                        principalTable: "GasStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHistoryFuel_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarHistoryFuel_Operators_OperatorModifyId",
                        column: x => x.OperatorModifyId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CarHistoryRepair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarId = table.Column<int>(type: "int", nullable: false),
                    MechanicId = table.Column<int>(type: "int", nullable: false),
                    Mileage = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OperatorCreateId = table.Column<int>(type: "int", nullable: false),
                    DateTimeAdd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperatorModifyId = table.Column<int>(type: "int", nullable: false),
                    DateTimeModify = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHistoryRepair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarHistoryRepair_Car_CarId",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarHistoryRepair_Mechanic_MechanicId",
                        column: x => x.MechanicId,
                        principalTable: "Mechanic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHistoryRepair_Operators_OperatorCreateId",
                        column: x => x.OperatorCreateId,
                        principalTable: "Operators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CarHistoryRepair_Operators_OperatorModifyId",
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
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    JobId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_TaskComments_Tasks_JobId",
                        column: x => x.JobId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_OperatorCreateId",
                table: "Car",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_OperatorModifyId",
                table: "Car",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryFuel_CarId",
                table: "CarHistoryFuel",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryFuel_GasStationId",
                table: "CarHistoryFuel",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryFuel_OperatorCreateId",
                table: "CarHistoryFuel",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryFuel_OperatorModifyId",
                table: "CarHistoryFuel",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryRepair_CarId",
                table: "CarHistoryRepair",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryRepair_MechanicId",
                table: "CarHistoryRepair",
                column: "MechanicId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryRepair_OperatorCreateId",
                table: "CarHistoryRepair",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryRepair_OperatorModifyId",
                table: "CarHistoryRepair",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStation_OperatorCreateId",
                table: "GasStation",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStation_OperatorModifyId",
                table: "GasStation",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanic_OperatorCreateId",
                table: "Mechanic",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Mechanic_OperatorModifyId",
                table: "Mechanic",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OperatorCreateId",
                table: "Projects",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_OperatorModifyId",
                table: "Projects",
                column: "OperatorModifyId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_JobId",
                table: "TaskComments",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_OperatorCreateId",
                table: "TaskComments",
                column: "OperatorCreateId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_OperatorModifyId",
                table: "TaskComments",
                column: "OperatorModifyId");

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
                name: "CarHistoryFuel");

            migrationBuilder.DropTable(
                name: "CarHistoryRepair");

            migrationBuilder.DropTable(
                name: "TaskComments");

            migrationBuilder.DropTable(
                name: "GasStation");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "Mechanic");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Operators");
        }
    }
}
