using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class GatherAndMushroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mushrooms",
                columns: table => new
                {
                    Idmushroom = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Edibility = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Class = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Marketprice = table.Column<int>(name: "Market_price", type: "int", nullable: true),
                    Mushroombirthplace = table.Column<string>(name: "Mushroom_birthplace", type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rarity = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mushrooms", x => x.Idmushroom);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gathers",
                columns: table => new
                {
                    IdGather = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Idmushroom = table.Column<int>(type: "int", nullable: false),
                    idmushroompicker = table.Column<int>(name: "idmushroom_picker", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gathers", x => x.IdGather);
                    table.ForeignKey(
                        name: "FK_Gathers_Mushrooms_Idmushroom",
                        column: x => x.Idmushroom,
                        principalTable: "Mushrooms",
                        principalColumn: "Idmushroom",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gathers_mushroom_picker_idmushroom_picker",
                        column: x => x.idmushroompicker,
                        principalTable: "mushroom_picker",
                        principalColumn: "idmushroom_picker",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Gathers_Idmushroom",
                table: "Gathers",
                column: "Idmushroom");

            migrationBuilder.CreateIndex(
                name: "IX_Gathers_idmushroom_picker",
                table: "Gathers",
                column: "idmushroom_picker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gathers");

            migrationBuilder.DropTable(
                name: "Mushrooms");
        }
    }
}
