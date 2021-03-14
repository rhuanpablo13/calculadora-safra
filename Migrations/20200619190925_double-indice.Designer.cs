﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using calculadora_api.Models;

namespace calculadora_api.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20200619190925_double-indice")]
    partial class doubleindice
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("calculadora_api.Models.ChequeEmpresarial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("contractRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dataBase")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dataBaseAtual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("encargosMonetarios")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("indiceBA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("indiceDB")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("indiceDataBase")
                        .HasColumnType("real");

                    b.Property<double>("indiceDataBaseAtual")
                        .HasColumnType("real");

                    b.Property<string>("infoParaCalculo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("lancamentos")
                        .HasColumnType("real");

                    b.Property<string>("tipoLancamento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ultimaAtualizacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("valorDevedor")
                        .HasColumnType("real");

                    b.Property<double>("valorDevedorAtualizado")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("ChequeEmpresarialItems");
                });

            modelBuilder.Entity("calculadora_api.Models.Indice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<string>("indice")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("valor")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.ToTable("IndiceItems");
                });

            modelBuilder.Entity("calculadora_api.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("acao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("contrato")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("data")
                        .HasColumnType("datetime2");

                    b.Property<string>("dataSimulacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("infoTabela")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pasta")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tipoContrato")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("usuario")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LogItems");
                });

            modelBuilder.Entity("calculadora_api.Models.ParceladoPre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("amortizacao")
                        .HasColumnType("real");

                    b.Property<string>("contractRef")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dataCalcAmor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("dataVencimento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("encargosMonetarios")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("indiceDCA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("indiceDV")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("indiceDataCalcAmor")
                        .HasColumnType("real");

                    b.Property<double>("indiceDataVencimento")
                        .HasColumnType("real");

                    b.Property<string>("infoParaCalculo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("nparcelas")
                        .HasColumnType("real");

                    b.Property<double>("parcelaInicial")
                        .HasColumnType("real");

                    b.Property<string>("status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("subtotal")
                        .HasColumnType("real");

                    b.Property<double>("totalDevedor")
                        .HasColumnType("real");

                    b.Property<string>("ultimaAtualizacao")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("valorNoVencimento")
                        .HasColumnType("real");

                    b.Property<string>("valorPMTVincenda")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ParceladoPreItems");
                });

            modelBuilder.Entity("calculadora_api.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Profile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserItems");
                });
#pragma warning restore 612, 618
        }
    }
}
