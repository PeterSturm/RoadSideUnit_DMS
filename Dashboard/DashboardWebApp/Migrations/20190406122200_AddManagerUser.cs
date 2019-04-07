using Microsoft.EntityFrameworkCore.Migrations;

namespace DashboardWebApp.Migrations
{
    public partial class AddManagerUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ManagerUsers",
                columns: table => new
                {
                    ManagerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Id = table.Column<int>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    SnmPv3Auth = table.Column<string>(nullable: true),
                    SnmPv3Priv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerUsers", x => new { x.ManagerId, x.Name });
                    table.ForeignKey(
                        name: "ForeignKey_ManagerUser_Manager",
                        column: x => x.ManagerId,
                        principalTable: "Managers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserManagerUsers",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    ManagerUserManagerId = table.Column<int>(nullable: false),
                    ManagerUserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserManagerUsers", x => new { x.UserId, x.ManagerUserManagerId, x.ManagerUserName });
                    table.ForeignKey(
                        name: "FK_UserManagerUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserManagerUsers_ManagerUsers_ManagerUserManagerId_ManagerU~",
                        columns: x => new { x.ManagerUserManagerId, x.ManagerUserName },
                        principalTable: "ManagerUsers",
                        principalColumns: new[] { "ManagerId", "Name" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserManagerUsers_ManagerUserManagerId_ManagerUserName",
                table: "UserManagerUsers",
                columns: new[] { "ManagerUserManagerId", "ManagerUserName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserManagerUsers");

            migrationBuilder.DropTable(
                name: "ManagerUsers");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
