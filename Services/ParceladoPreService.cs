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

        private bool calcularMulta = true;

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
                p.encargosMonetarios.multa = this.calcularMulta
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
        
        

        public Parcelado calcularAmortizacao(List<InfoParaAmortizacao> amortizacoes, TotaisRodape rodape, TotalParcelasVencidas totalParcelasVencidas, List<RegistroParcela> tabelaParcelados, string contractRef) {

            if (amortizacoes == null || amortizacoes.Count == 0) {
                return null;
            }

            if (tabelaParcelados == null || tabelaParcelados.Count == 0) {
                return null;
            }

            Tabela<RegistroParcela> tabela = new Tabela<RegistroParcela>();
            tabela.adicionarRegistro(tabelaParcelados[0]);
            //tabelaParcelados.RemoveAt(0);

            List<InfoParaAmortizacao> amortizacoesFinal = new List<InfoParaAmortizacao>();
            RegistroParcela parceladoRegistro;
            bool proximo = true;
            int i = 0;
            int controle = 5;

            List<InfoParaAmortizacao> amortizacoesAux = amortizacoes;
            // removendo as amortizações do tipo "Final" e separando para serem calculadas por último
            for (int j = 0; j < amortizacoes.Count; j++) {
                InfoParaAmortizacao amortizacao = amortizacoes[j];
                if (amortizacao.tipo == "Final") {
                    amortizacoesFinal.Add(amortizacao);
                    amortizacoesAux.RemoveAt(j);
                }
            }
            amortizacoes = amortizacoesAux;


            do {
                parceladoRegistro = tabela.getRegistros()[i];
                var valorDevedor = parceladoRegistro.vincenda ? parceladoRegistro.valorPMTVincenda : parceladoRegistro.subtotal;
                string situacao = parceladoRegistro.status;
                
                if (situacao == "Pago" || situacao == "Amortizada") {
                    i++; // vai pro próximo registro
                    tabela.adicionarRegistro(tabelaParcelados[i]);
                    continue;
                }


                // Aberto
                for (int j = 0; j < amortizacoes.Count; j++)
                {
                    InfoParaAmortizacao amortizacao = amortizacoes[j];

                    if (valorDevedor == amortizacao.saldo_devedor) {
                        parceladoRegistro.amortizacao = amortizacao.saldo_devedor;
                        parceladoRegistro.status = "Pago";
                        parceladoRegistro.totalDevedor = 0;
                        tabela.getRegistros()[i] = parceladoRegistro; // atualiza o registro no array
                        amortizacoes.RemoveAt(j);
                        break;
                    }

                    if (valorDevedor > amortizacao.saldo_devedor) {
                        Console.WriteLine("Entrou aqui");
                        parceladoRegistro.dataCalcAmor = amortizacao.data_vencimento; //ok
                        parceladoRegistro.amortizacao = amortizacao.saldo_devedor; //ok
                        parceladoRegistro.status = "Amortizada"; //ok
                        parceladoRegistro = this.calcular(parceladoRegistro);
                        tabela.getRegistros()[i] = parceladoRegistro; // atualiza o registro no array
                        RegistroParcela novoParcelado = novaParcela(parceladoRegistro, amortizacao, true);
                        tabela.adicionarRegistro(novoParcelado);
                        proximo = true;
                        i++;
                        break;
                    }

                    if (valorDevedor < amortizacao.saldo_devedor) {
                        proximo = false;
                        Console.WriteLine("Amortização é maior que a parcela");
                    }

                }

                controle--;
                if (controle == 0) {Console.WriteLine("Bateu o limite"); break;}
            } while (proximo);


            Console.WriteLine(tabela);
            return null;

            // TotaisParcelas totais = new TotaisParcelas();
            // totais.totalParcelasVencidas = totalParcelasVencidas;

            // Parcelado parcelado = new Parcelado(contractRef, tabela.getRegistros(), infoParaCalculo, amortizacoes, rodape, totais);
            
            // if (amortizacoesFinal.Count > 0) {
            //     TotaisRodape rodapeNovo = new TotaisRodape();
            //     foreach (var amt in amortizacoesFinal) {
            //         rodapeNovo = calcularTotaisRodape( totalParcelasVencidas, amt.saldo_devedor, rodape.subtotal, rodape.honorario, rodape.amortizacao, rodape.multa, rodape.total);
            //     }
            //     parcelado.rodape = rodapeNovo;
            // }
            // return parcelado;

        }


        private RegistroParcela calcularDiferenciada(InfoParaAmortizacao amortizacao, RegistroParcela parcelado) {
            
                var valorDevedor = parcelado.vincenda ? parcelado.valorPMTVincenda : parcelado.subtotal;
                string situacao = parcelado.status;

                if (situacao == "Pago") return parcelado;

                if (valorDevedor == amortizacao.saldo_devedor) {
                    parcelado.amortizacao = amortizacao.saldo_devedor;
                    parcelado.status = "Pago";
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
            
            this.calcularMulta = false;
            novaParcela = this.calcular(novaParcela);
            novaParcela.status = "Aberto";
            this.calcularMulta = true;

            // TODO: Verificar se o status é Pago
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