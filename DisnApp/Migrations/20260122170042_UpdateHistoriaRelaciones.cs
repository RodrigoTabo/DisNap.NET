using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DisnApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateHistoriaRelaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId",
                table: "HistoriaVistas");

            migrationBuilder.DropForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId1",
                table: "HistoriaVistas");

            migrationBuilder.DropIndex(
                name: "IX_HistoriaVistas_HistoriaId1",
                table: "HistoriaVistas");

            migrationBuilder.DropColumn(
                name: "HistoriaId1",
                table: "HistoriaVistas");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Historias",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId",
                table: "HistoriaVistas",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId",
                table: "HistoriaVistas");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Historias");

            migrationBuilder.AddColumn<int>(
                name: "HistoriaId1",
                table: "HistoriaVistas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoriaVistas_HistoriaId1",
                table: "HistoriaVistas",
                column: "HistoriaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId",
                table: "HistoriaVistas",
                column: "HistoriaId",
                principalTable: "Historias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HistoriaVistas_Historias_HistoriaId1",
                table: "HistoriaVistas",
                column: "HistoriaId1",
                principalTable: "Historias",
                principalColumn: "Id");
        }
    }
}
