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

        private readonly IndiceController indiceController;

        private InfoParaCalculo infoParaCalculo;

        public ParceladoPreService(IndiceController indiceController, InfoParaCalculo infoParaCalculo)
        {
            this.indiceController = indiceController;
            this.infoParaCalculo = infoParaCalculo;
        }


        public TabelaParcelados calcular(string contractRef, List<Parcela> parcelas)
        {
            Parcelado parcelado;
            TabelaParcelados table = new TabelaParcelados();
            foreach (var parcela in parcelas)
            {
                parcelado = new Parcelado();
                parcelado.carregarDadosEntrada(contractRef, infoParaCalculo, parcela);
                parcelado = calcular(parcelado);
                table.adicionarRegistro(parcelado);
            }
            return table;
        }


        public TotaisParcelas calcularTotaisParcelas(TabelaParcelados tabelaParcelados) {

            TotaisParcelas totais = new TotaisParcelas();
            foreach (Parcelado parcelado in tabelaParcelados.getRegistros())
            {
                if (parcelado.vincenda) {
                    totais.totalParcelasVincendas.valorPMTVincenda += parcelado.valorPMTVincenda;
                    totais.totalParcelasVincendas.totalDevedor += parcelado.totalDevedor;
                } else {
                    totais.totalParcelasVencidas.valorNoVencimento += parcelado.valorNoVencimento;
                    totais.totalParcelasVencidas.correcaoPeloIndice += parcelado.encargosMonetarios.correcaoPeloIndice;
                    totais.totalParcelasVencidas.money += parcelado.encargosMonetarios.jurosAm.moneyValue;
                    totais.totalParcelasVencidas.somaMulta += parcelado.encargosMonetarios.multa;
                    totais.totalParcelasVencidas.subTotal += parcelado.subtotal;
                    totais.totalParcelasVencidas.amortizacao += parcelado.amortizacao;
                    totais.totalParcelasVencidas.totalDevedor += parcelado.totalDevedor;
                }
            }
            return totais;
        }
        

        private Parcelado calcular(Parcelado p)
        {
            //dias
            p.encargosMonetarios.jurosAm.dias = UService.numberOfDays(p.dataCalcAmor, p.dataVencimento);
            //indiceDataVencimento
            p.indiceDataVencimento = UService.getIndice(
                p.indiceDV, p.dataVencimento, this.infoParaCalculo.formIndiceEncargos, indiceController
            );
            //indiceDataCalcAmor
            p.indiceDataCalcAmor = UService.getIndice(
                p.indiceDCA, p.dataCalcAmor, this.infoParaCalculo.formIndiceEncargos, indiceController
            );

            //correcaoPeloIndice
            p.encargosMonetarios.correcaoPeloIndice = (
                (p.valorNoVencimento / p.indiceDataVencimento) * p.indiceDataCalcAmor
            ) - p.valorNoVencimento;
            //percentsJuros
            p.encargosMonetarios.jurosAm.percentsJuros = (this.infoParaCalculo.formJuros / 30) * p.encargosMonetarios.jurosAm.dias;
            //moneyValue
            p.encargosMonetarios.jurosAm.moneyValue = ((p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice) / 30) * p.encargosMonetarios.jurosAm.dias * (this.infoParaCalculo.formJuros / 100);
            
            //multa
            p.encargosMonetarios.multa = (p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue) * (this.infoParaCalculo.formMulta / 100);
            //subtotal
            p.subtotal = p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa;
            //valorPMTVincenda
            p.valorPMTVincenda = p.valorNoVencimento * this.infoParaCalculo.desagio;
            
            p.amortizacao = 0;

            //totalDevedor
            p.totalDevedor = !p.vincenda 
            ? p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa + p.amortizacao 
            : p.valorPMTVincenda;

            return p;
        }


        public void calcularAmortizacao(List<InfoParaAmortizacao> amortizacoes, TotaisRodape rodape, TotalParcelasVencidas totalParcelasVencidas) {

            if (amortizacoes == null) return;

            foreach (var amt in amortizacoes)
            {
                switch (amt.tipo)
                {
                    // TODO: validar quando amortização for maior que o total devedor
                    case "Final":
                    rodape = this.calcularTotaisRodape(
                        totalParcelasVencidas, 
                        amt.saldo_devedor,
                        rodape.subtotal,
                        rodape.honorario,
                        rodape.amortizacao,
                        rodape.multa,
                        rodape.total
                    );
                    break;

                    case "Data Diferenciada":

                    break;

                    default:
                    break;
                }
            }            
            Console.WriteLine("calculando amortização");
            Console.WriteLine(rodape);
        }


        public void calcularDiferenciada(InfoParaAmortizacao amortizacao, RetornoParcelado retornoParcelado) {
            
            
            // foreach (Parcelado parcelado in retornoParcelado.tabela)
            // {
            //     var valorDevedor = parcelado.vincenda ? parcelado.valorPMTVincenda : parcelado.subtotal;
                

            //     valorDevedor == amortizacao.saldo_devedor;
            //         parcelado.amortizacao = amortizacao.saldo_devedor;
            //         parcelado.status = 'Amortizado';
            //         parcelado.totalDevedor = 0;

            //     valorDevedor > amortizacao.saldo_devedor;
            //         parcelado.amortizacao = amortizacao.saldo_devedor;
            //         parcelado.status = 'Aberto';
            //         parcelado.totalDevedor = valorDevedor - amortizacao.saldo_devedor;                   
            //         // chamar a funcao novaParcela()

            //     valorDevedor < amortizacao.saldo_devedor;
            //         parcelado.amortizacao = valorDevedor;
            //         parcelado.status = 'Amortizado';
            //         parcelado.totalDevedor = 0;
            //         // o que sobrar da amortização, jogar na parcela de baixo
            //         // chamar a funcao novaParcela()

            // }

            // recalcular os totais das colunas e o rodapé,
            // revisar o calculo do rodapé considerando a amortizaçao
        }


        public void novaParcela(Parcelado parcelado, InfoParaAmortizacao infoParaAmortizacao) {
            // criar uma nova parcela
            // Parcelado novaParcela = new Parcelado();

            // //colocar decimal crescente na parcela recebida por parametro
            // novaParcela.dataCalcAmor = parcelado.dataCalcAmor;
            // parcelado.dataCalcAmor = infoParaAmortizacao.data_vencimento;            
            // novaParcela.parcela.dataVencimento = infoParaAmortizacao.data_vencimento;
            
            // novaParcela.indiceDataVencimento = indiceController.GetIndiceItemsByDate(parcelado.indiceDV, novaParcela.parcela.dataVencimento);
            // novaParcela.indiceDataCalcAmor = indiceController.GetIndiceItemsByDate(parcelado.indiceDCA, novaParcela.dataCalcAmor);
            // novaParcela.parcela.valorNoVencimento = parcelado.totalDevedor;



        }



        public TotaisRodape calcularTotaisRodape(TotaisParcelas totaisParcelas)
        {
            return this.calcularTotaisRodape(totaisParcelas.totalParcelasVencidas, 0, 0, 0, 0, 0, 0);
        }


        private TotaisRodape calcularTotaisRodape(TotalParcelasVencidas totalParcelasVencidas, float saldoDevedorAmortizacao, float subtotal, float honorarios, float amortizacao, float multa, float total)
        {

            // Console.WriteLine(totalParcelasVencidas);
            // Console.WriteLine(saldoDevedorAmortizacao);
            // Console.WriteLine(subtotal);
            // Console.WriteLine(honorarios);
            // Console.WriteLine(amortizacao);
            // Console.WriteLine(multa);
            // Console.WriteLine(total);
            Console.WriteLine("-------------");

            subtotal = totalParcelasVencidas.totalDevedor;

            // Console.WriteLine(amortizacao);
            // Console.WriteLine(saldoDevedorAmortizacao);
            // Console.WriteLine("---");

            amortizacao += saldoDevedorAmortizacao;

            // honorarios = valorDevedorAtualizado 
            honorarios = subtotal * (this.infoParaCalculo.formHonorarios / 100);
            // multa = ((valorDevedorAtualizado + honorarios) * multa_sob_contrato grupo 2 / 100
            multa = (subtotal + honorarios) * (this.infoParaCalculo.formMultaSobContrato / 100);
            // total_grandtotal = tmulta_sob_contrato + honorarios + valorDevedorAtualizado;
            total = (honorarios + subtotal + multa) - amortizacao;

            // calcular a amortização
            return new TotaisRodape(subtotal, honorarios, multa, total, amortizacao);
        }
    }
}