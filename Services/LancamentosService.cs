using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using Converter;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using calculadora_api.Controllers;

namespace calculadora_api.Services
{
    public class LancamentosService
    {

        private IndiceController indiceController;

        public LancamentosService(IndiceController indiceController) {
            this.indiceController = indiceController;
        }
        
        public Tabela calcular(JObject dados) {
            DadosLancamento dadosLancamento = new DadosLancamento(dados);
            dadosLancamento.parse();

            Tabela tabela = new Tabela();
            tabela.carregarRegistros(dados);

            if (tabela.temRegistros()) {
                return calcular(dadosLancamento, tabela);
            }
            var cheque = new ChequeEmpresarialBack();
            cheque.copyFromDadosLancamento(dadosLancamento);
            
            tabela.adicionarRegistro(calcular(cheque));
            return tabela;
        }



        public Tabela calcular(DadosLancamento dadosLancamento, Tabela table) {
            ChequeEmpresarialBack registroSuperior = table.getUltimoRegistro();
            if (registroSuperior != null) {
                ChequeEmpresarialBack novoRegistro = new ChequeEmpresarialBack();
                novoRegistro.copyFromDadosLancamento(dadosLancamento);
                novoRegistro.dataBase = registroSuperior.dataBaseAtual;
                novoRegistro.encargosMonetarios.multa = -1; // multa só é calculada na primeira linha
                novoRegistro.valorDevedor = registroSuperior.valorDevedorAtualizado;
                novoRegistro.indiceBA = dadosLancamento.formIndice == null ? registroSuperior.indiceBA : dadosLancamento.formIndice;
                novoRegistro.indiceDB = novoRegistro.indiceBA;
                table.adicionarRegistro(calcular(novoRegistro));
                return table;
            }
            return null;
        }



        private ChequeEmpresarialBack calcular(ChequeEmpresarialBack dadosLancamento) {
            
            dadosLancamento.encargosMonetarios.jurosAm.dias = numberOfDays(dadosLancamento.dataBase, dadosLancamento.dataBaseAtual);
            dadosLancamento.indiceDataBase = getIndiceDataBase(dadosLancamento.indiceDB, dadosLancamento.dataBase, dadosLancamento.infoParaCalculo);
            dadosLancamento.indiceDataBaseAtual = getIndiceDataBase(dadosLancamento.indiceBA, dadosLancamento.dataBaseAtual, dadosLancamento.infoParaCalculo);
            
            if (dadosLancamento.indiceDB == "Encargos Contratuais %")
                dadosLancamento.encargosMonetarios.correcaoPeloIndice = ((dadosLancamento.valorDevedor * (dadosLancamento.indiceDataBaseAtual / 100)) / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias;
            else
                dadosLancamento.encargosMonetarios.correcaoPeloIndice = (dadosLancamento.valorDevedor / dadosLancamento.indiceDataBase) * dadosLancamento.indiceDataBaseAtual - dadosLancamento.valorDevedor;
            
            dadosLancamento.encargosMonetarios.jurosAm.percentsJuros = (dadosLancamento.infoParaCalculo.formJuros / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias;
            dadosLancamento.encargosMonetarios.jurosAm.moneyValue = ((dadosLancamento.valorDevedor + dadosLancamento.encargosMonetarios.correcaoPeloIndice) / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias * (dadosLancamento.infoParaCalculo.formJuros / 100);
            
            // só é calculado aqui na primeira linha
            if (dadosLancamento.encargosMonetarios.multa != -1)
                dadosLancamento.encargosMonetarios.multa = (dadosLancamento.valorDevedor + dadosLancamento.encargosMonetarios.correcaoPeloIndice + dadosLancamento.encargosMonetarios.jurosAm.moneyValue) * (dadosLancamento.infoParaCalculo.formMulta / 100);
            
            if (dadosLancamento.tipoLancamento == "debit")
                dadosLancamento.valorDevedorAtualizado = dadosLancamento.valorDevedor + dadosLancamento.encargosMonetarios.correcaoPeloIndice + dadosLancamento.encargosMonetarios.jurosAm.moneyValue + dadosLancamento.encargosMonetarios.multa + dadosLancamento.lancamentos;
            else
                dadosLancamento.valorDevedorAtualizado = (dadosLancamento.valorDevedor + dadosLancamento.encargosMonetarios.correcaoPeloIndice) - ((dadosLancamento.lancamentos - dadosLancamento.encargosMonetarios.multa) - dadosLancamento.encargosMonetarios.jurosAm.moneyValue);

            Console.WriteLine("*********** dados calculados ***************");
            Console.WriteLine(dadosLancamento.ToString());

            return dadosLancamento;
        }


        public Totais calcularTotais(Tabela table) {

            double subtotal = 0;
            double honorarios = 0;
            double multa = 0;
            double total = 0;

            if (!table.temRegistros()) {
                return null;
            }

            ChequeEmpresarialBack cb = table.getUltimoRegistro();
            subtotal = cb.valorDevedorAtualizado;
            
            // honorarios = valorDevedorAtualizado 
            honorarios = subtotal * (cb.infoParaCalculo.formHonorarios / 100);
            // multa = ((valorDevedorAtualizado + honorarios) * multa_sob_contrato grupo 2 / 100
            multa = (subtotal + honorarios) * (cb.infoParaCalculo.formMultaSobContrato / 100);
            // total_grandtotal = tmulta_sob_contrato + honorarios + valorDevedorAtualizado;
            total = honorarios + subtotal + multa;

            return new Totais(subtotal, honorarios, multa, total);
        }





        private double getIndiceDataBase(string indice, DateTime date, InfoParaCalculo formDefaultValues) {
            return indiceController.getIndiceDataBase(
                indice,
                date,
                formDefaultValues.formIndiceEncargos
            );
        }

        private int numberOfDays(DateTime minor, DateTime major) {
            int days = major.Subtract(minor).Days;
            return days >= 0 ? days : minor.Subtract(major).Days;
        }
    }
}