using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SNMPManager.Migrations
{
    public partial class InitalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RSUs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IP = table.Column<IPAddress>(nullable: false),
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
                name: "ManagerLogs",
                columns: table => new
                {
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerLogs", x => new { x.TimeStamp, x.TypeId });
                    table.ForeignKey(
                        name: "FK_ManagerLogs_LogTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrapLogs",
                columns: table => new
                {
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    SourceRSU = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrapLogs", x => new { x.TimeStamp, x.TypeId, x.SourceRSU });
                    table.UniqueConstraint("AK_TrapLogs_SourceRSU_TimeStamp_TypeId", x => new { x.SourceRSU, x.TimeStamp, x.TypeId });
                    table.ForeignKey(
                        name: "FK_TrapLogs_LogTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "LogTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManagerLogs_TypeId",
                table: "ManagerLogs",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrapLogs_TypeId",
                table: "TrapLogs",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManagerLogs");

            migrationBuilder.DropTable(
                name: "RSUs");

            migrationBuilder.DropTable(
                name: "TrapLogs");

            migrationBuilder.DropTable(
                name: "LogTypes");
        }
    }
}
