using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockPulse.Migrations
{
    public partial class addtodbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole");

            migrationBuilder.RenameTable(
                name: "EmployeeRole",
                newName: "EmployeeRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles",
                columns: new[] { "EmployeeEmail", "Name" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRoles_Employees_EmployeeEmail",
                table: "EmployeeRoles",
                column: "EmployeeEmail",
                principalTable: "Employees",
                principalColumn: "person",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeRoles_Employees_EmployeeEmail",
                table: "EmployeeRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeRoles",
                table: "EmployeeRoles");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                newName: "EmployeeRole");

            migrationBuilder.AddColumn<string>(
                name: "PersonEmail",
                table: "EmployeeRole",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeRole",
                table: "EmployeeRole",
                columns: new[] { "EmployeeEmail", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRole_PersonEmail",
                table: "EmployeeRole",
                column: "PersonEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeRole_Employees_PersonEmail",
                table: "EmployeeRole",
                column: "PersonEmail",
                principalTable: "Employees",
                principalColumn: "person");
        }
    }
}
