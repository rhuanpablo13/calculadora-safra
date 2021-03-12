using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace calculadora_api.Migrations
{
    public partial class AddUsersToDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChequeEmpresarialItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataBase = table.Column<string>(nullable: true),
                    indiceDB = table.Column<string>(nullable: true),
                    indiceDataBase = table.Column<float>(nullable: false),
                    indiceBA = table.Column<string>(nullable: true),
                    indiceDataBaseAtual = table.Column<float>(nullable: false),
                    dataBaseAtual = table.Column<string>(nullable: true),
                    valorDevedor = table.Column<float>(nullable: false),
                    encargosMonetarios = table.Column<string>(nullable: true),
                    lancamentos = table.Column<float>(nullable: false),
                    tipoLancamento = table.Column<string>(nullable: true),
                    valorDevedorAtualizado = table.Column<float>(nullable: false),
                    contractRef = table.Column<string>(nullable: true),
                    ultimaAtualizacao = table.Column<string>(nullable: true),
                    infoParaCalculo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChequeEmpresarialItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    indice = table.Column<string>(nullable: true),
                    data = table.Column<string>(nullable: true),
                    valor = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndiceItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParceladoPreItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nparcelas = table.Column<float>(nullable: false),
                    parcelaInicial = table.Column<float>(nullable: false),
                    indiceDataVencimento = table.Column<float>(nullable: false),
                    indiceDataCalcAmor = table.Column<float>(nullable: false),
                    valorNoVencimento = table.Column<float>(nullable: false),
                    subtotal = table.Column<float>(nullable: false),
                    amortizacao = table.Column<float>(nullable: false),
                    totalDevedor = table.Column<float>(nullable: false),
                    contractRef = table.Column<string>(nullable: true),
                    dataVencimento = table.Column<string>(nullable: true),
                    indiceDV = table.Column<string>(nullable: true),
                    indiceDCA = table.Column<string>(nullable: true),
                    dataCalcAmor = table.Column<string>(nullable: true),
                    valorPMTVincenda = table.Column<string>(nullable: true),
                    status = table.Column<string>(nullable: true),
                    ultimaAtualizacao = table.Column<string>(nullable: true),
                    encargosMonetarios = table.Column<string>(nullable: true),
                    infoParaCalculo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParceladoPreItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Profile = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserItems", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChequeEmpresarialItems");

            migrationBuilder.DropTable(
                name: "IndiceItems");

            migrationBuilder.DropTable(
                name: "ParceladoPreItems");

            migrationBuilder.DropTable(
                name: "UserItems");
        }
    }
}
