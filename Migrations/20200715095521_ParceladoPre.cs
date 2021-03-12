using Microsoft.EntityFrameworkCore.Migrations;

namespace calculadora_api.Migrations
{
    public partial class ParceladoPre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "nparcelas",
                table: "ParceladoPreItems",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "nparcelas",
                table: "ParceladoPreItems",
                type: "real",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
