using Microsoft.EntityFrameworkCore.Migrations;

namespace calculadora_api.Migrations
{
    public partial class LogMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    data = table.Column<string>(nullable: true),
                    usuario = table.Column<string>(nullable: true),
                    pasta = table.Column<string>(nullable: true),
                    contrato = table.Column<string>(nullable: true),
                    tipoContrato = table.Column<string>(nullable: true),
                    dataSimulacao = table.Column<string>(nullable: true),
                    infoTabela = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogItems");
        }
    }
}
