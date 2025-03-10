using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developers.Stream.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class include_streamer_identifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "Streamers",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.NewGuid());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Streamers");
        }
    }
}
