using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardWebApp.Migrations
{
    public partial class RemoveAltKeyFromManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Managers_IP_Port",
                table: "Managers");

            migrationBuilder.AlterColumn<IPAddress>(
                name: "IP",
                table: "Managers",
                nullable: true,
                oldClrType: typeof(IPAddress));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<IPAddress>(
                name: "IP",
                table: "Managers",
                nullable: false,
                oldClrType: typeof(IPAddress),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Managers_IP_Port",
                table: "Managers",
                columns: new[] { "IP", "Port" });
        }
    }
}
