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

        public Tabela<RegistroParcela> tabela {get; set;} // TODO: mudar para privado

        

        public ParceladoPreService(IndiceController indiceController, InfoParaCalculo infoParaCalculo)
        {
            this.indiceController = indiceController;
            this.infoParaCalculo = infoParaCalculo;
        }


        float a = 0;

        public TotaisParcelas calcularTotaisParcelas(Tabela<RegistroParcela> tabelaParcelados) {

            TotaisParcelas totais = new TotaisParcelas();
            foreach (RegistroParcela parcelado in tabelaParcelados.getRegistros())
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
            totais.round(2);
            return totais;
        }
        

        public RegistroParcela calcular(RegistroParcela p)
        {
            //dias
            if (!p.vincenda) {
                p.encargosMonetarios.jurosAm.dias = UService.numberOfDays(p.dataCalcAmor, p.dataVencimento);
            }

            //indiceDataVencimento
            p.indiceDataVencimento = UService.getIndice(
                p.indiceDV, p.dataVencimento, this.infoParaCalculo.formIndiceEncargos, indiceController
            );
            //indiceDataCalcAmor
            p.indiceDataCalcAmor = UService.getIndice(
                p.indiceDCA, p.dataCalcAmor, this.infoParaCalculo.formIndiceEncargos, indiceController
            );

            //correcaoPeloIndice
            if (!p.vincenda) {
                p.encargosMonetarios.correcaoPeloIndice = MathF.Round((
                    (p.valorNoVencimento / p.indiceDataVencimento) * p.indiceDataCalcAmor
                ) - p.valorNoVencimento, 2);
            }

            if (!p.vincenda) {
                //percentsJuros
                p.encargosMonetarios.jurosAm.percentsJuros = MathF.Round((this.infoParaCalculo.formJuros / 30) * p.encargosMonetarios.jurosAm.dias, 2);
                //moneyValue
                p.encargosMonetarios.jurosAm.moneyValue = MathF.Round(((p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice) / 30) * p.encargosMonetarios.jurosAm.dias * (this.infoParaCalculo.formJuros / 100), 2);
                //multa
                p.encargosMonetarios.multa = p.status != "Amortizada"
                    ? MathF.Round((p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue) * (this.infoParaCalculo.formMulta / 100), 2)
                    : 0;

                //subtotal
                p.subtotal = MathF.Round(p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa, 2);
            }
            
            //valorPMTVincenda
            p.valorPMTVincenda = MathF.Round(p.valorNoVencimento * (this.infoParaCalculo.formDesagio / 100), 2);
            
            //totalDevedor
            if (!p.vincenda && p.status != "Amortizada") {
                p.totalDevedor = 
                    p.valorNoVencimento + 
                    p.encargosMonetarios.correcaoPeloIndice +
                    p.encargosMonetarios.jurosAm.moneyValue +
                    p.encargosMonetarios.multa +
                    p.amortizacao;
            }

            if (!p.vincenda && p.status == "Amortizada") {
                p.totalDevedor = p.subtotal - p.amortizacao;
            }
            // TODO: tratar quando for vincenda e/ou amortizada
            // : p.valorPMTVincenda;            
            p.totalDevedor = MathF.Round(p.totalDevedor, 2);

            return p;
        }


        public Parcelado calcularAmortizacao(List<InfoParaAmortizacao> amortizacoes, TotaisRodape rodape, TotalParcelasVencidas totalParcelasVencidas, List<RegistroParcela> tabelaParcelados) {

            Parcelado parcelado = new Parcelado();
            if (amortizacoes == null) {
                return null;
            }


            Tabela<RegistroParcela> tabela = new Tabela<RegistroParcela>();            
            List<InfoParaAmortizacao> amortizacoesDoRodape = new List<InfoParaAmortizacao>();

            foreach (var amt in amortizacoes)
            {
                switch (amt.tipo)
                {
                    // TODO: validar quando amortização for maior que o total devedor
                    case "Final":
                    amortizacoesDoRodape.Add(amt);
                    break;

                    case "Data Diferenciada":
                    RegistroParcela parceladoTemp;
                    foreach (RegistroParcela registro in tabelaParcelados) {
                        parceladoTemp = calcularDiferenciada(amt, registro);
                        if (parceladoTemp.status == "Amortizado") {
                            tabela.adicionarRegistro(parceladoTemp);
                            RegistroParcela novoParcelado = novaParcela(parceladoTemp, amt, true);
                            tabela.adicionarRegistro(novoParcelado);
                        }
                    }
                    break;

                    default:
                    //calcularDiferenciada(amt, tabelaParcelados);
                    break;
                }
            }

            TotaisParcelas totais = new TotaisParcelas();
            totais.totalParcelasVencidas = totalParcelasVencidas;
            parcelado.totais = totais;
            parcelado.tabela = tabela.getRegistros();            
            parcelado.rodape = rodape;

            if (amortizacoesDoRodape.Count > 0) {
                TotaisRodape rodapeNovo = new TotaisRodape();
                foreach (var amt in amortizacoesDoRodape) {
                    rodapeNovo = calcularTotaisRodape( totalParcelasVencidas, amt.saldo_devedor, rodapeNovo.subtotal, rodapeNovo.honorario, rodapeNovo.amortizacao, rodapeNovo.multa, rodapeNovo.total);
                }
                parcelado.rodape = rodapeNovo;
            }
            return parcelado;
        }


        private RegistroParcela calcularDiferenciada(InfoParaAmortizacao amortizacao, RegistroParcela parcelado) {
            
                var valorDevedor = parcelado.vincenda ? parcelado.valorPMTVincenda : parcelado.subtotal;
                string situacao = parcelado.status;

                if (situacao == "Paga") return parcelado;

                if (valorDevedor == amortizacao.saldo_devedor) {
                    parcelado.amortizacao = amortizacao.saldo_devedor;
                    parcelado.status = "Amortizado";
                    parcelado.totalDevedor = 0;
                    return parcelado;

                } else if (valorDevedor > amortizacao.saldo_devedor) {
                    parcelado.dataCalcAmor = amortizacao.data_vencimento; //ok
                    parcelado.amortizacao = amortizacao.saldo_devedor; //ok
                    parcelado.status = "Amortizada"; //ok
                    return this.calcular(parcelado);
                    
                } else {
                    Console.WriteLine("<");
                    return new RegistroParcela();
                }

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


            // recalcular os totais das colunas e o rodapé,
            // revisar o calculo do rodapé considerando a amortizaçao
        }


        public RegistroParcela novaParcela(RegistroParcela parcelado, InfoParaAmortizacao infoParaAmortizacao, bool maior) {

            RegistroParcela novaParcela = new RegistroParcela();
            // TODO: aqui a parcela vai receber o . alguma coisa
            float nrParcela = float.Parse(parcelado.nparcelas) + 0.1f;
            novaParcela.nparcelas = nrParcela.ToString();
            novaParcela.dataVencimento = infoParaAmortizacao.data_vencimento;
            novaParcela.dataCalcAmor = infoParaCalculo.formDataCalculo;
            novaParcela.indiceDV = parcelado.indiceDV;
            novaParcela.indiceDCA = parcelado.indiceDCA;
            novaParcela.contractRef = parcelado.contractRef;
            novaParcela.valorNoVencimento = parcelado.totalDevedor;
            
            novaParcela = this.calcular(novaParcela);
            // TODO: Verificar se o status é Pago
            novaParcela.status = "Aberto";
            return novaParcela;
        }



        public TotaisRodape calcularTotaisRodape(TotaisParcelas totaisParcelas)
        {
            return this.calcularTotaisRodape(totaisParcelas.totalParcelasVencidas, 0, 0, 0, 0, 0, 0);
        }


        private TotaisRodape calcularTotaisRodape(TotalParcelasVencidas totalParcelasVencidas, float saldoDevedorAmortizacao, float subtotal, float honorarios, float amortizacao, float multa, float total)
        {

            subtotal = totalParcelasVencidas.totalDevedor;
            amortizacao += saldoDevedorAmortizacao;

            // honorarios = valorDevedorAtualizado 
            honorarios = MathF.Round(subtotal * (this.infoParaCalculo.formHonorarios / 100), 2);

            // multa = ((valorDevedorAtualizado + honorarios) * multa_sob_contrato grupo 2 / 100
            multa = MathF.Round((subtotal + honorarios) * (this.infoParaCalculo.formMultaSobContrato / 100), 2);
            
            // total_grandtotal = tmulta_sob_contrato + honorarios + valorDevedorAtualizado;
            total = MathF.Round((honorarios + subtotal + multa) - amortizacao, 2);

            // calcular a amortização
            return new TotaisRodape(subtotal, honorarios, multa, total, amortizacao);
        }
    }
}