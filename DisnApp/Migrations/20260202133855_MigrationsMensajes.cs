using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisnApp.Migrations
{
    /// <inheritdoc />
    public partial class MigrationsMensajes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Eliminada",
                table: "ConversacionUsuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "EliminadaAt",
                table: "ConversacionUsuarios",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Eliminada",
                table: "ConversacionUsuarios");

            migrationBuilder.DropColumn(
                name: "EliminadaAt",
                table: "ConversacionUsuarios");
        }
    }
}
