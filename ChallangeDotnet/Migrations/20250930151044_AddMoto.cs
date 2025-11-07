using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChallangeDotnet.Migrations
{
    /// <inheritdoc />
    public partial class AddMoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_moto",
                columns: table => new
                {
                    Id = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    Modelo = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Ano = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_moto", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_moto");
        }
    }
}
