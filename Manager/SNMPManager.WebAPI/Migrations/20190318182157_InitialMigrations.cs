using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SNMPManager.Core.Enumerations;

namespace SNMPManager.WebAPI.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:log_type", "db,security,apicall,snmp");

            migrationBuilder.CreateTable(
                name: "ManagerLogs",
                columns: table => new
                {
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Type = table.Column<LogType>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerLogs", x => new { x.TimeStamp, x.Type });
                });

            migrationBuilder.CreateTable(
                name: "ManagerSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Timeout = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RSUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IP = table.Column<IPAddress>(nullable: false),
                    Port = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Latitude = table.Column<double>(nullable: false),
                    Longitude = table.Column<double>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    MIBVersion = table.Column<string>(maxLength: 32, nullable: true),
                    FirmwareVersion = table.Column<string>(maxLength: 32, nullable: true),
                    LocationDescription = table.Column<string>(maxLength: 140, nullable: true),
                    Manufacturer = table.Column<string>(maxLength: 32, nullable: true),
                    NotificationIP = table.Column<IPAddress>(nullable: false),
                    NotificationPort = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RSUs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrapLogs",
                columns: table => new
                {
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Type = table.Column<LogType>(nullable: false),
                    SourceRSU = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrapLogs", x => new { x.TimeStamp, x.Type, x.SourceRSU });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    UserName = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: true),
                    RoleId = table.Column<int>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    SNMPv3Auth = table.Column<string>(nullable: true),
                    SNMPv3Priv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_UserName", x => x.UserName);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerLogs");

            migrationBuilder.DropTable(
                name: "ManagerSettings");

            migrationBuilder.DropTable(
                name: "RSUs");

            migrationBuilder.DropTable(
                name: "TrapLogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.Sql("DROP TYPE log_type");
        }
    }
}
