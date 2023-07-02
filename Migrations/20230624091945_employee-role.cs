using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockPulse.Migrations
{
    public partial class employeerole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeRole",
                columns: table => new
                {
                    EmployeeEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRole", x => new { x.EmployeeEmail, x.Name });
                    table.ForeignKey(
                        name: "FK_EmployeeRole_Employees_PersonEmail",
                        column: x => x.EmployeeEmail,
                        principalTable: "Employees",
                        principalColumn: "person");
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
