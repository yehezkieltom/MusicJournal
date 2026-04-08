using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicJournal.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackStatusEnumAndFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Tracks",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Album",
                table: "Tracks",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Tracks",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReleaseYear",
                table: "Tracks",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Album",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "ReleaseYear",
                table: "Tracks");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Tracks",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
