using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MVP.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DBMaterial",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    category = table.Column<string>(nullable: true),
                    countName = table.Column<string>(nullable: true),
                    planCount = table.Column<int>(nullable: false),
                    factCount = table.Column<int>(nullable: false),
                    planPrice = table.Column<int>(nullable: false),
                    factPrice = table.Column<int>(nullable: false),
                    status = table.Column<string>(nullable: true),
                    planPayDate = table.Column<DateTime>(nullable: false),
                    factPayDate = table.Column<DateTime>(nullable: false),
                    taskId = table.Column<int>(nullable: false),
                    taskName = table.Column<string>(nullable: true),
                    projId = table.Column<int>(nullable: false),
                    provider = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBMaterial", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBProject",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    adr = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    planStartDate = table.Column<DateTime>(nullable: false),
                    factStartDate = table.Column<DateTime>(nullable: false),
                    planFinishDate = table.Column<DateTime>(nullable: false),
                    factFinishDate = table.Column<DateTime>(nullable: false),
                    planWorkPrice = table.Column<int>(nullable: false),
                    factWorkPrice = table.Column<int>(nullable: false),
                    planMaterialPrice = table.Column<int>(nullable: false),
                    factMaterialPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBProject", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBTask",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    prijId = table.Column<int>(nullable: false),
                    planStartDate = table.Column<DateTime>(nullable: false),
                    factStartDate = table.Column<DateTime>(nullable: false),
                    planFinishDate = table.Column<DateTime>(nullable: false),
                    factFinishDate = table.Column<DateTime>(nullable: false),
                    planPayDate = table.Column<DateTime>(nullable: false),
                    factPayDate = table.Column<DateTime>(nullable: false),
                    planedPrice = table.Column<int>(nullable: false),
                    factPrice = table.Column<int>(nullable: false),
                    parentTaskName = table.Column<string>(nullable: true),
                    parentTaskId = table.Column<int>(nullable: false),
                    materials = table.Column<string>(nullable: true),
                    necesseMaterials = table.Column<string>(nullable: true),
                    supervisor = table.Column<string>(nullable: true),
                    planedMaterialPrice = table.Column<int>(nullable: false),
                    factMaterialPrice = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBTask", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "DBUser",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    mail = table.Column<string>(nullable: true),
                    name = table.Column<string>(nullable: true),
                    seurname = table.Column<string>(nullable: true),
                    post = table.Column<string>(nullable: true),
                    passvord = table.Column<string>(nullable: true),
                    token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DBUser", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DBMaterial");

            migrationBuilder.DropTable(
                name: "DBProject");

            migrationBuilder.DropTable(
                name: "DBTask");

            migrationBuilder.DropTable(
                name: "DBUser");
        }
    }
}
