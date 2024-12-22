using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Valhalla_v3.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_Operators_OperatorCreateId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_Operators_OperatorModifyId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryFuel_Operators_OperatorCreateId",
                table: "CarHistoryFuel");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryFuel_Operators_OperatorModifyId",
                table: "CarHistoryFuel");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_Operators_OperatorCreateId",
                table: "CarHistoryRepair");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_Operators_OperatorModifyId",
                table: "CarHistoryRepair");

            migrationBuilder.DropForeignKey(
                name: "FK_GasStation_Operators_OperatorCreateId",
                table: "GasStation");

            migrationBuilder.DropForeignKey(
                name: "FK_GasStation_Operators_OperatorModifyId",
                table: "GasStation");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanic_Operators_OperatorCreateId",
                table: "Mechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanic_Operators_OperatorModifyId",
                table: "Mechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Operators_OperatorCreateId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Operators_OperatorModifyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Operators_OperatorCreateId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Operators_OperatorModifyId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Operators_OperatorCreateId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Operators_OperatorModifyId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Operators",
                table: "Operators");

            migrationBuilder.RenameTable(
                name: "Operators",
                newName: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                table: "AspNetUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AspNetUsers_OperatorCreateId",
                table: "Car",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_AspNetUsers_OperatorModifyId",
                table: "Car",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryFuel_AspNetUsers_OperatorCreateId",
                table: "CarHistoryFuel",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryFuel_AspNetUsers_OperatorModifyId",
                table: "CarHistoryFuel",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_AspNetUsers_OperatorCreateId",
                table: "CarHistoryRepair",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_AspNetUsers_OperatorModifyId",
                table: "CarHistoryRepair",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStation_AspNetUsers_OperatorCreateId",
                table: "GasStation",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStation_AspNetUsers_OperatorModifyId",
                table: "GasStation",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanic_AspNetUsers_OperatorCreateId",
                table: "Mechanic",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanic_AspNetUsers_OperatorModifyId",
                table: "Mechanic",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_OperatorCreateId",
                table: "Projects",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_OperatorModifyId",
                table: "Projects",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_AspNetUsers_OperatorCreateId",
                table: "TaskComments",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_AspNetUsers_OperatorModifyId",
                table: "TaskComments",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_OperatorCreateId",
                table: "Tasks",
                column: "OperatorCreateId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_OperatorModifyId",
                table: "Tasks",
                column: "OperatorModifyId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Car_AspNetUsers_OperatorCreateId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_Car_AspNetUsers_OperatorModifyId",
                table: "Car");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryFuel_AspNetUsers_OperatorCreateId",
                table: "CarHistoryFuel");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryFuel_AspNetUsers_OperatorModifyId",
                table: "CarHistoryFuel");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_AspNetUsers_OperatorCreateId",
                table: "CarHistoryRepair");

            migrationBuilder.DropForeignKey(
                name: "FK_CarHistoryRepair_AspNetUsers_OperatorModifyId",
                table: "CarHistoryRepair");

            migrationBuilder.DropForeignKey(
                name: "FK_GasStation_AspNetUsers_OperatorCreateId",
                table: "GasStation");

            migrationBuilder.DropForeignKey(
                name: "FK_GasStation_AspNetUsers_OperatorModifyId",
                table: "GasStation");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanic_AspNetUsers_OperatorCreateId",
                table: "Mechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_Mechanic_AspNetUsers_OperatorModifyId",
                table: "Mechanic");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_OperatorCreateId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_OperatorModifyId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_AspNetUsers_OperatorCreateId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_AspNetUsers_OperatorModifyId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_OperatorCreateId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_OperatorModifyId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "EmailIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "Operators");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Operators",
                table: "Operators",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Operators_OperatorCreateId",
                table: "Car",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Car_Operators_OperatorModifyId",
                table: "Car",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryFuel_Operators_OperatorCreateId",
                table: "CarHistoryFuel",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryFuel_Operators_OperatorModifyId",
                table: "CarHistoryFuel",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_Operators_OperatorCreateId",
                table: "CarHistoryRepair",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarHistoryRepair_Operators_OperatorModifyId",
                table: "CarHistoryRepair",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStation_Operators_OperatorCreateId",
                table: "GasStation",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStation_Operators_OperatorModifyId",
                table: "GasStation",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanic_Operators_OperatorCreateId",
                table: "Mechanic",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mechanic_Operators_OperatorModifyId",
                table: "Mechanic",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Operators_OperatorCreateId",
                table: "Projects",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Operators_OperatorModifyId",
                table: "Projects",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Operators_OperatorCreateId",
                table: "TaskComments",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Operators_OperatorModifyId",
                table: "TaskComments",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Operators_OperatorCreateId",
                table: "Tasks",
                column: "OperatorCreateId",
                principalTable: "Operators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Operators_OperatorModifyId",
                table: "Tasks",
                column: "OperatorModifyId",
                principalTable: "Operators",
                principalColumn: "Id");
        }
    }
}
