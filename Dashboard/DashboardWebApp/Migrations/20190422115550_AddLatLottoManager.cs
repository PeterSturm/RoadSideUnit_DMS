using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardWebApp.Migrations
{
    public partial class AddLatLottoManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Managers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Managers",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Managers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 47.592700000000001, 19.036999999999999 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Managers");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Managers");
        }
    }
}
