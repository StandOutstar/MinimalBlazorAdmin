using Microsoft.EntityFrameworkCore.Migrations;

namespace MinimalBlazorAdmin.Server.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b796d0ea-2c96-4dad-86ac-cd5bcf3fb644", "a682e498-4a3f-4fe4-acbd-62a46675eda2", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a033e1ed-1a90-4fb9-a2f2-b28dc5cb70a6", "8bf00aaa-b09d-4775-a4b4-b3885c50fe46", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a033e1ed-1a90-4fb9-a2f2-b28dc5cb70a6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b796d0ea-2c96-4dad-86ac-cd5bcf3fb644");
        }
    }
}
