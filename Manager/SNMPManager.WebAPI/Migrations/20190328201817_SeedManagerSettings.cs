using Microsoft.EntityFrameworkCore.Migrations;

namespace SNMPManager.WebAPI.Migrations
{
    public partial class SeedManagerSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ManagerSettings",
                columns: new[] { "Id", "Timeout" },
                values: new object[] { 1, 2000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ManagerSettings",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
