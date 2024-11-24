using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valhalla_v3.Migrations
{
    /// <inheritdoc />
    public partial class AddDateToCar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryFuel_GasStation_GasStationId1",
                table: "CarHistoryFuel");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId",
                table: "CarHistoryRepair");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId1",
                table: "CarHistoryRepair");

            migrationBuilder.DropIndex(
                name: "IX_CarHistoryRepair_MechanicId1",
                table: "CarHistoryRepair");

            migrationBuilder.DropIndex(
                name: "IX_CarHistoryFuel_GasStationId1",
                table: "CarHistoryFuel");

            migrationBuilder.DropColumn(
                name: "MechanicId1",
                table: "CarHistoryRepair");

            migrationBuilder.DropColumn(
                name: "GasStationId1",
                table: "CarHistoryFuel");

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceCost",
                table: "Car",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceDate",
                table: "Car",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SurveyCost",
                table: "Car",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SurveyDate",
                table: "Car",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId",
                table: "CarHistoryRepair",
                column: "MechanicId",
                principalTable: "Mechanic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId",
                table: "CarHistoryRepair");

            migrationBuilder.DropColumn(
                name: "InsuranceCost",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "InsuranceDate",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "SurveyCost",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "SurveyDate",
                table: "Car");

            migrationBuilder.AddColumn<int>(
                name: "MechanicId1",
                table: "CarHistoryRepair",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GasStationId1",
                table: "CarHistoryFuel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryRepair_MechanicId1",
                table: "CarHistoryRepair",
                column: "MechanicId1");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryFuel_GasStationId1",
                table: "CarHistoryFuel",
                column: "GasStationId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryFuel_GasStation_GasStationId1",
                table: "CarHistoryFuel",
                column: "GasStationId1",
                principalTable: "GasStation",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId",
                table: "CarHistoryRepair",
                column: "MechanicId",
                principalTable: "Mechanic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_Mechanic_MechanicId1",
                table: "CarHistoryRepair",
                column: "MechanicId1",
                principalTable: "Mechanic",
                principalColumn: "Id");
        }
    }
}
