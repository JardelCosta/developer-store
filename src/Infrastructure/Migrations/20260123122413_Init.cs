using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations;

/// <inheritdoc />
public partial class Init : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "sale",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                sale_number = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                branch_id = table.Column<Guid>(type: "uuid", nullable: false),
                customer_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                branch_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                sale_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                total_amount = table.Column<decimal>(type: "numeric", nullable: false),
                is_cancelled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table => table.PrimaryKey("pk_sale", x => x.id));

        migrationBuilder.CreateTable(
            name: "sale_item",
            schema: "public",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                sale_id = table.Column<Guid>(type: "uuid", nullable: false),
                product_id = table.Column<Guid>(type: "uuid", nullable: false),
                product_description = table.Column<string>(type: "text", nullable: false),
                quantity = table.Column<int>(type: "integer", nullable: false),
                unit_price = table.Column<decimal>(type: "numeric", nullable: false),
                discount = table.Column<decimal>(type: "numeric", nullable: false),
                total_amount = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                is_cancelled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_sale_item", x => x.id);
                table.ForeignKey(
                    name: "fk_sale_item_sale_sale_id",
                    column: x => x.sale_id,
                    principalSchema: "public",
                    principalTable: "sale",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "ix_sale_item_sale_id",
            schema: "public",
            table: "sale_item",
            column: "sale_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "sale_item",
            schema: "public");

        migrationBuilder.DropTable(
            name: "sale",
            schema: "public");
    }
}
