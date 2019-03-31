using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardWebApp.Migrations
{
    public partial class ManagerSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "IP", "Name", "Port" },
                values: new object[] { 1, System.Net.IPAddress.Parse("127.0.0.1"), "Local", 51467 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
