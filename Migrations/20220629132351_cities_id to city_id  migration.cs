using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Migrations
{
    public partial class cities_idtocity_idmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobilAku_Cities_cities_id",
                table: "MobilAku");

            migrationBuilder.RenameColumn(
                name: "cities_id",
                table: "MobilAku",
                newName: "city_id");

            migrationBuilder.RenameColumn(
                name: "n_of_partial_age",
                table: "MobilAku",
                newName: "n_of_partial_charge");

            migrationBuilder.RenameIndex(
                name: "IX_MobilAku_cities_id",
                table: "MobilAku",
                newName: "IX_MobilAku_city_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MobilAku_Cities_city_id",
                table: "MobilAku",
                column: "city_id",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MobilAku_Cities_city_id",
                table: "MobilAku");

            migrationBuilder.RenameColumn(
                name: "city_id",
                table: "MobilAku",
                newName: "cities_id");

            migrationBuilder.RenameColumn(
                name: "n_of_partial_charge",
                table: "MobilAku",
                newName: "n_of_partial_age");

            migrationBuilder.RenameIndex(
                name: "IX_MobilAku_city_id",
                table: "MobilAku",
                newName: "IX_MobilAku_cities_id");

            migrationBuilder.AddForeignKey(
                name: "FK_MobilAku_Cities_cities_id",
                table: "MobilAku",
                column: "cities_id",
                principalTable: "Cities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
