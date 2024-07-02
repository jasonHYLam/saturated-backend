using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace color_picker_server.Migrations
{
    /// <inheritdoc />
    public partial class ModifyNoteModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "XOrdinateAsFraction",
                table: "Notes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "YOrdinateAsFraction",
                table: "Notes",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XOrdinateAsFraction",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "YOrdinateAsFraction",
                table: "Notes");
        }
    }
}
