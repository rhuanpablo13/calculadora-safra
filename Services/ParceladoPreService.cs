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
    public class ParceladoPreService
    {

        private IndiceController indiceController;

        public ParceladoPreService(IndiceController indiceController) {
            this.indiceController = indiceController;
        }
        

        public Tabela calcular(JObject dados) {
            DadosParceladoPre dadosParcela = new DadosParceladoPre(dados);
            dadosParcela.parse();

            Tabela tabela = new Tabela();
            tabela.carregarRegistros(dados);

            if (tabela.temRegistros()) {
                return calcular(dadosParcela, tabela);
            }
            var parcela = new ParceladoPre();
            //cheque.copyFromDadosLancamento(dadosParcela);
            
            tabela.adicionarRegistro(calcular(parcela));
            return tabela;
        }



        public Tabela calcular(DadosParceladoPre dadosLancamento, Tabela table) {
            // ParceladoPre novoRegistro = new ParceladoPre();
            // novoRegistro.copyFromDadosLancamento(dadosLancamento);
            // novoRegistro.dataBase = registroSuperior.dataBaseAtual;
            // novoRegistro.encargosMonetarios.multa = -1; // multa só é calculada na primeira linha
            // novoRegistro.valorDevedor = registroSuperior.valorDevedorAtualizado;
            // novoRegistro.indiceBA = dadosLancamento.formIndice == null ? registroSuperior.indiceBA : dadosLancamento.formIndice;
            // novoRegistro.indiceDB = novoRegistro.indiceBA;
            // table.adicionarRegistro(calcular(novoRegistro));
            return table;
        }



        private ChequeEmpresarialBack calcular(ParceladoPre dadosLancamento) {
            
            // dadosLancamento.encargosMonetarios.jurosAm.dias = numberOfDays(dadosLancamento.dataCalcAmor, dadosLancamento.dataVencimento);
            
            // dadosLancamento.indiceDataVencimento = getIndiceDataBase(dadosLancamento.indiceDV, dadosLancamento.dataVencimento, dadosLancamento.infoParaCalculo);
            // dadosLancamento.indiceDataCalcAmor = getIndiceDataBase(dadosLancamento.indiceDCA, dadosLancamento.dataCalcAmor, dadosLancamento.infoParaCalculo);
            
            // if (dadosLancamento.indiceDV == "Encargos Contratuais %")
            //     dadosLancamento.encargosMonetarios.correcaoPeloIndice = ((dadosLancamento.valorNoVencimento * (dadosLancamento.indiceDataBaseAtual / 100)) / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias;
            // else
            //     dadosLancamento.encargosMonetarios.correcaoPeloIndice = (dadosLancamento.valorDevedor / dadosLancamento.indiceDataBase) * dadosLancamento.indiceDataBaseAtual - dadosLancamento.valorDevedor;
            
            // dadosLancamento.encargosMonetarios.jurosAm.percentsJuros = (dadosLancamento.infoParaCalculo.formJuros / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias;
            // dadosLancamento.encargosMonetarios.jurosAm.moneyValue = ((dadosLancamento.valorNoVencimento + dadosLancamento.encargosMonetarios.correcaoPeloIndice) / 30) * dadosLancamento.encargosMonetarios.jurosAm.dias * (dadosLancamento.infoParaCalculo.formJuros / 100);
            
            // dadosLancamento.encargosMonetarios.multa = ((dadosLancamento.valorNoVencimento + dadosLancamento.encargosMonetarios.correcaoPeloIndice + dadosLancamento.encargosMonetarios.jurosAm.moneyValue + (dadosLancamento.infoParaCalculo.formMulta / 100);
            // dadosLancamento.subtotal = dadosLancamento.valorNoVencimento + dadosLancamento.encargosMonetarios.correcaoPeloIndice + dadosLancamento.encargosMonetarios.jurosAm.moneyValue + dadosLancamento.encargosMonetarios.multa;
            // dadosLancamento.valorPMTVincenda = dadosLancamento.valorNoVencimento * dadosLancamento.infoParaCalculo.desagio;
            // // dadosLancamento.amortizacao = null;
            // dadosLancamento.totalDevedor = dadosLancamento.vincenda ? dadosLancamento.valorNoVencimento + dadosLancamento.encargosMonetarios.correcaoPeloIndice + money + dadosLancamento.encargosMonetarios.multa + dadosLancamento.amortizacao : dadosLancamento.valorPMTVincenda;
            // dadosLancamento.status = status;

            // if (vincenda) {
            //     totalParcelasVencidas = [
            //         valorNoVencimento += dadosLancamento.valorNoVencimento,
            //         correcaoPeloIndice += dadosLancamento.correcaoPeloIndice,
            //         money += money,
            //         multa += multa,
            //         subTotal += subTotal,
            //         amortizacao += amortizacao,
            //         totalDevedor += totalDevedor
            //     ];

            // } else {
            //     totalParcelasVincendas = [
            //         valorPMTVincenda += valorPMTVincenda,
            //         totalDevedor += totalDevedor
            //     ];
            // }


            // Console.WriteLine("*********** dados calculados ***************");
            // Console.WriteLine(dadosLancamento.ToString());

            // return dadosLancamento;
            return null;
        }


        public Totais calcularTotais(Tabela table) {

            float subtotal = 0;
            float honorarios = 0;
            float multa = 0;
            float total = 0;

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





        private float getIndiceDataBase(string indice, DateTime date, InfoParaCalculo formDefaultValues) {
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