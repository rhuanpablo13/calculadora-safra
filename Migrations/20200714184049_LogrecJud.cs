using Microsoft.EntityFrameworkCore.Migrations;

namespace calculadora_api.Migrations
{
    public partial class LogrecJud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "recuperacaoJudicial",
                table: "LogItems",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "recuperacaoJudicial",
                table: "LogItems");
        }
    }
}
