using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisnApp.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenPerfil",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenPerfil",
                table: "AspNetUsers");
        }
    }
}
