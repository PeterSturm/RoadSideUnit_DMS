using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SNMPManager.WebAPI.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RSUs",
                columns: new[] { "Id", "Active", "FirmwareVersion", "IP", "Latitude", "LocationDescription", "Longitude", "MIBVersion", "Manufacturer", "Name", "NotificationIP", "NotificationPort", "Port" },
                values: new object[,]
                {
                    { 1, true, "", System.Net.IPAddress.Parse("172.168.45.27"), 17.449999999999999, "", 24.120000000000001, "", "Commsignia", "TestRSU", System.Net.IPAddress.Parse("186.56.123.84"), 161, 162 },
                    { 2, true, "", System.Net.IPAddress.Parse("112.111.45.89"), 19.449999999999999, "", 45.119999999999997, "", "Commsignia", "RSUuu", System.Net.IPAddress.Parse("186.56.123.84"), 161, 162 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName" },
                values: new object[] { 1, "Péter", "Sturm", 1, "authpass012", "privpass012", "test", "sturm" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RSUs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RSUs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
