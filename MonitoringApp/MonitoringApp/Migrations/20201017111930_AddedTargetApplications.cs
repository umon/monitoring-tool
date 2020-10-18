using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringApp.Migrations
{
    public partial class AddedTargetApplications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TargetApplications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Interval = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    NotificationMail = table.Column<string>(nullable: true),
                    UserGuid = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetApplications", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TargetApplications");
        }
    }
}
