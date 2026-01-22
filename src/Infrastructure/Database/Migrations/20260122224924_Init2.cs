using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Database.Migrations;

/// <inheritdoc />
public partial class Init2 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "external_identity",
            schema: "public");

        migrationBuilder.AddColumn<string>(
            name: "product_description",
            schema: "public",
            table: "sale_item",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "product_id",
            schema: "public",
            table: "sale_item",
            type: "uuid",
            nullable: false,
            defaultValue: Guid.Empty);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "product_description",
            schema: "public",
            table: "sale_item");

        migrationBuilder.DropColumn(
            name: "product_id",
            schema: "public",
            table: "sale_item");

        migrationBuilder.CreateTable(
            name: "external_identity",
            schema: "public",
            columns: table => new
            {
                description = table.Column<string>(type: "text", nullable: false),
                external_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
            });
    }
}
