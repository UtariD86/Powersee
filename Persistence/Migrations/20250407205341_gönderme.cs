using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class gönderme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Departments",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CalismaTuru",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Managarıd",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UniqueCode",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "CalismaTuru",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Managarıd",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "UniqueCode",
                table: "Departments");
        }
    }
}
