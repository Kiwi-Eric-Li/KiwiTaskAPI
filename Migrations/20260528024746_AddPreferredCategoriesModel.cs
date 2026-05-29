using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KiwiTaskAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPreferredCategoriesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "preferred_categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    category_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_preferred_categories", x => x.id);
                    table.ForeignKey(
                        name: "FK_preferred_categories_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            

            migrationBuilder.CreateIndex(
                name: "IX_preferred_categories_user_id",
                table: "preferred_categories",
                column: "user_id");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.DropTable(
                name: "preferred_categories");

            
        }
    }
}
