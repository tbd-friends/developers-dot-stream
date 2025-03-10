using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Developers.Stream.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class modify_channel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Channels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Channels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
