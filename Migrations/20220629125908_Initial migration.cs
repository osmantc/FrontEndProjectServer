using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class Initialmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    latitude = table.Column<string>(type: "TEXT", nullable: false),
                    longitude = table.Column<string>(type: "TEXT", nullable: false),
                    region_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                    table.ForeignKey(
                        name: "FK_Cities_Regions_region_id",
                        column: x => x.region_id,
                        principalTable: "Regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MobilAku",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    tenant_id = table.Column<string>(type: "TEXT", nullable: false),
                    report_file_process_id = table.Column<int>(type: "INTEGER", nullable: true),
                    location = table.Column<string>(type: "TEXT", nullable: false),
                    asset_num = table.Column<string>(type: "TEXT", nullable: false),
                    n_of_ac = table.Column<int>(type: "INTEGER", nullable: false),
                    n_of_ne = table.Column<int>(type: "INTEGER", nullable: false),
                    battery_age = table.Column<string>(type: "TEXT", nullable: false),
                    n_of_partial_age = table.Column<int>(type: "INTEGER", nullable: false),
                    n_of_generator = table.Column<int>(type: "INTEGER", nullable: false),
                    n_of_air_con = table.Column<int>(type: "INTEGER", nullable: false),
                    max_ac_duration = table.Column<string>(type: "TEXT", nullable: false),
                    mx_afad = table.Column<int>(type: "INTEGER", nullable: false),
                    totalpower_loc = table.Column<string>(type: "TEXT", nullable: false),
                    ideal_working_hour = table.Column<string>(type: "TEXT", nullable: false),
                    back_sites = table.Column<string>(type: "TEXT", nullable: false),
                    technology = table.Column<string>(type: "TEXT", nullable: false),
                    remaining_battery_lifetime = table.Column<string>(type: "TEXT", nullable: false),
                    remaining_battery_lifetime_cast_int = table.Column<int>(type: "INTEGER", nullable: false),
                    recommendation = table.Column<string>(type: "TEXT", nullable: false),
                    additional_info = table.Column<string>(type: "TEXT", nullable: false),
                    current_date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    region_id = table.Column<int>(type: "INTEGER", nullable: false),
                    cities_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobilAku", x => x.id);
                    table.ForeignKey(
                        name: "FK_MobilAku_Cities_cities_id",
                        column: x => x.cities_id,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MobilAku_Regions_region_id",
                        column: x => x.region_id,
                        principalTable: "Regions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_region_id",
                table: "Cities",
                column: "region_id");

            migrationBuilder.CreateIndex(
                name: "IX_MobilAku_cities_id",
                table: "MobilAku",
                column: "cities_id");

            migrationBuilder.CreateIndex(
                name: "IX_MobilAku_region_id",
                table: "MobilAku",
                column: "region_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MobilAku");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
