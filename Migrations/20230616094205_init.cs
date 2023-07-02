using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StockPulse.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Particle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pronouns = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    product_num = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    purchase_price = table.Column<double>(type: "float", nullable: false),
                    selling_price = table.Column<double>(type: "float", nullable: false),
                    image_path = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.product_num);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    person = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.person);
                    table.ForeignKey(
                        name: "FK_Customers_Persons_person",
                        column: x => x.person,
                        principalTable: "Persons",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    person = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    montly_salary = table.Column<double>(type: "float", nullable: false),
                    employee_since = table.Column<DateTime>(type: "datetime2", nullable: false),
                    profile_picture_path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salted_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.person);
                    table.ForeignKey(
                        name: "FK_Employees_Persons_person",
                        column: x => x.person,
                        principalTable: "Persons",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    order_num = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    discount_price = table.Column<double>(type: "float", nullable: true),
                    delivery_notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.order_num);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_customer",
                        column: x => x.customer,
                        principalTable: "Customers",
                        principalColumn: "person",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmployeePersonEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Name);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_EmployeePersonEmail",
                        column: x => x.EmployeePersonEmail,
                        principalTable: "Employees",
                        principalColumn: "person");
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    manager = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warehouses_Employees_manager",
                        column: x => x.manager,
                        principalTable: "Employees",
                        principalColumn: "person",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderLines",
                columns: table => new
                {
                    order_num = table.Column<int>(type: "int", nullable: false),
                    product_num = table.Column<int>(type: "int", nullable: false),
                    warehouse = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderLines", x => new { x.product_num, x.order_num });
                    table.ForeignKey(
                        name: "FK_OrderLines_Orders_order_num",
                        column: x => x.order_num,
                        principalTable: "Orders",
                        principalColumn: "order_num",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Products_product_num",
                        column: x => x.product_num,
                        principalTable: "Products",
                        principalColumn: "product_num",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderLines_Warehouses_warehouse",
                        column: x => x.warehouse,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductStocks",
                columns: table => new
                {
                    warehouse = table.Column<int>(type: "int", nullable: false),
                    product_num = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStocks", x => new { x.product_num, x.warehouse });
                    table.ForeignKey(
                        name: "FK_ProductStocks_Products_product_num",
                        column: x => x.product_num,
                        principalTable: "Products",
                        principalColumn: "product_num",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStocks_Warehouses_warehouse",
                        column: x => x.warehouse,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Name", "EmployeePersonEmail" },
                values: new object[,]
                {
                    { "Admin", null },
                    { "Customer", null },
                    { "Employee", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_order_num",
                table: "OrderLines",
                column: "order_num");

            migrationBuilder.CreateIndex(
                name: "IX_OrderLines_warehouse",
                table: "OrderLines",
                column: "warehouse");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_customer",
                table: "Orders",
                column: "customer");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStocks_warehouse",
                table: "ProductStocks",
                column: "warehouse");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_EmployeePersonEmail",
                table: "Roles",
                column: "EmployeePersonEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_manager",
                table: "Warehouses",
                column: "manager");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderLines");

            migrationBuilder.DropTable(
                name: "ProductStocks");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
