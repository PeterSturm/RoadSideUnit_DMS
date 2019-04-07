using Microsoft.EntityFrameworkCore.Migrations;

namespace SNMPManager.WebAPI.Migrations
{
    public partial class UserModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Monitor" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName" },
                values: new object[] { 1, 1, "authpass012", "privpass012", "Adminpass01", "admin" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName" },
                values: new object[] { 2, 2, "authpass012", "privpass012", "Monitorpass01", "monitor" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "LastName", "RoleId", "SNMPv3Auth", "SNMPv3Priv", "Token", "UserName" },
                values: new object[] { 1, "Péter", "Sturm", 1, "authpass012", "privpass012", "test", "sturm" });
        }
    }
}
