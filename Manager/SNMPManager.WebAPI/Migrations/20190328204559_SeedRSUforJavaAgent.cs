using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNMPManager.WebAPI.Migrations
{
    public partial class SeedRSUforJavaAgent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RSUs",
                columns: new[] { "Id", "Active", "FirmwareVersion", "IP", "Latitude", "LocationDescription", "Longitude", "MIBVersion", "Manufacturer", "Name", "NotificationIP", "NotificationPort", "Port" },
                values: new object[] { 3, true, "", System.Net.IPAddress.Parse("127.0.0.1"), 13.449999999999999, "", 32.119999999999997, "", "Commsignia", "RSUjavaagent", System.Net.IPAddress.Parse("127.0.0.1"), 162, 161 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RSUs",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
