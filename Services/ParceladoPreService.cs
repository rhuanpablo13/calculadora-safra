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
                if (p.nparcelas.Contains(",")) {
                    p.encargosMonetarios.multa = 0;
                } else {
                    p.encargosMonetarios.multa = MathF.Round((p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue) * (this.infoParaCalculo.formMulta / 100), 2);
                }

                //subtotal
                p.subtotal = MathF.Round(p.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa, 2);
            }
            
            //float desagio = p.valorNoVencimento
            //valorPMTVincenda
            if (p.vincenda) {
                Console.WriteLine("Datas > amort: " + p.dataCalcAmor + " venc: " + p.dataVencimento);
                var dias = UService.numberOfDays(p.dataCalcAmor, p.dataVencimento);
                Console.WriteLine("dias > " + -dias);
                //var juros = p.encargosMonetarios.jurosAm.percentsJuros;
                var juros = infoParaCalculo.formJuros / 100;
                Console.WriteLine("Juros > " + juros);
                Console.WriteLine("formDesagio > " + this.infoParaCalculo.formDesagio);
                Console.WriteLine("p.valorNoVencimento > " + p.valorNoVencimento);
                
                var desagio = MathF.Pow((this.infoParaCalculo.formDesagio / 100) +1, -dias / 30);
                Console.WriteLine("Desagio > " + desagio);
                p.valorPMTVincenda = MathF.Round(p.valorNoVencimento * (desagio), 2) - p.amortizacao;
                Console.WriteLine("p.amortizacao > " + p.amortizacao);
                Console.WriteLine("p.valorPMTVincenda > " + p.valorPMTVincenda);
            }
            
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
            
            if (p.vincenda)
                p.totalDevedor = p.valorPMTVincenda;
            // TODO: tratar quando for vincenda e/ou amortizada
            // : p.valorPMTVincenda;            
            p.totalDevedor = MathF.Round(p.totalDevedor, 2);

            return p;
        }
        
        

        public Tabela<RegistroParcela> calcularAmortizacao(List<InfoParaAmortizacao> amortizacoes, TotaisRodape rodape, TotalParcelasVencidas totalParcelasVencidas, List<RegistroParcela> tabelaParcelados, string contractRef) {

            if (amortizacoes == null || amortizacoes.Count == 0) {
                return null;
            }

            if (tabelaParcelados == null || tabelaParcelados.Count == 0) {
                return null;
            }

            
            Tabela<RegistroParcela> tabela = new Tabela<RegistroParcela>();
            
            List<InfoParaAmortizacao> amortizacoesFinal = new List<InfoParaAmortizacao>();
            RegistroParcela parceladoRegistro;
            bool proximo = true;
            bool calcularSobreAmortizada = false;
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
            int qtdAmort = amortizacoes.Count;

            do {

                if (tabelaParcelados.Count == 0 && !calcularSobreAmortizada) {
                    break;
                }

                if (calcularSobreAmortizada) {
                    Console.WriteLine("calcularSobreAmortizad");
                    Console.WriteLine(i);
                    Console.WriteLine(tabela.ToString());
                    //break;
                    parceladoRegistro = tabela.get(i);
                } else {
                    parceladoRegistro = tabelaParcelados[i];
                    tabelaParcelados.RemoveAt(i);
                }


                var valorDevedor = parceladoRegistro.vincenda ? parceladoRegistro.valorPMTVincenda : parceladoRegistro.subtotal;
                string situacao = parceladoRegistro.status;
                
                if (situacao == "Pago" || situacao == "Amortizada") {
                    Console.WriteLine("Parcela Paga ou Amortizada");
                    Console.WriteLine(parceladoRegistro);

                    tabela.adicionarRegistro(tabelaParcelados[i]);
                    tabelaParcelados.RemoveAt(i);

                    i++; // vai pro próximo registro
                    if (tabelaParcelados[i] != null) {
                        continue;
                    }
                    break;
                }


                // Aberto
                for (int j = 0; j < amortizacoes.Count; j++)
                {
                    InfoParaAmortizacao amortizacao = amortizacoes[j];
                    Console.WriteLine("amortização: ");
                    Console.WriteLine(amortizacao);
                    Console.WriteLine();

                    if (valorDevedor == amortizacao.saldo_devedor) {
                        Console.WriteLine("valor devedor e amort são iguais");
                        Console.WriteLine(parceladoRegistro);

                        parceladoRegistro.amortizacao = amortizacao.saldo_devedor;
                        parceladoRegistro.status = "Pago";
                        parceladoRegistro.totalDevedor = 0;
                        tabela.update(i, parceladoRegistro); // atualiza o registro no array
                        amortizacoes.RemoveAt(j);
                        break;
                    }

                    // amortizando um valor menor que a parcela
                    if (amortizacao.saldo_devedor < valorDevedor) {
                        Console.WriteLine("amortizando um valor menor que a parcela");
                        if (tabela.temRegistros() == false) Console.WriteLine("a tabela está vazia");

                        parceladoRegistro.dataCalcAmor = amortizacao.data_vencimento; //ok
                        parceladoRegistro.amortizacao = amortizacao.saldo_devedor; //ok
                        parceladoRegistro.status = "Amortizada"; //ok
                        // Console.WriteLine(parceladoRegistro);
                        parceladoRegistro = this.calcular(parceladoRegistro);
                        if (tabela.exists(i)) {
                            Console.WriteLine("o registro já existe, vamos atualizar");
                            // Console.WriteLine(parceladoRegistro);

                            tabela.update(i, parceladoRegistro); // atualiza o registro no array
                        } else {
                            Console.WriteLine("O registro não existe, vamos inserir");
                            // Console.WriteLine(parceladoRegistro);

                            tabela.adicionarRegistro(parceladoRegistro);
                        }
                        Console.WriteLine("calculando a nova parcela");
                        RegistroParcela novoParcelado;
                        if (j == 0 && amortizacoes.Count == qtdAmort){
                            novoParcelado = novaParcela(parceladoRegistro, amortizacao, true, true);
                        } else {
                            novoParcelado = novaParcela(parceladoRegistro, amortizacao, true, false);
                        }
                        // Console.WriteLine(novoParcelado);
                        tabela.adicionarRegistro(novoParcelado);
                        amortizacoes.RemoveAt(j);

                        // se ainda tiver amortizações para calcular
                        if (amortizacoes.Count > j) {
                            // Console.WriteLine("ainda tem amortizações");
                            calcularSobreAmortizada = true;
                            proximo = true;
                            i++;
                            break;
                        } else {
                            // Console.WriteLine("não tem mais amortizações");
                            if (tabelaParcelados.Count > 0) {
                                // Console.WriteLine("ainda tem parcelas sobrando, então vamos adicionar ao final da tabela");                                
                            }
                            tabelaParcelados.ForEach(a => tabela.adicionarRegistro(a));
                            calcularSobreAmortizada = false;
                            proximo = false;
                        }
                    }

                    if (valorDevedor < amortizacao.saldo_devedor) {
                        proximo = false;
                        // Console.WriteLine("Amortização é maior que a parcela");
                        break;
                    }

                }

                controle--;
                if (controle == 0) {Console.WriteLine("Bateu o limite"); break;}
            } while (proximo);


            // Console.WriteLine(tabela);
            return tabela;

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


        public RegistroParcela novaParcela(RegistroParcela parcelado, InfoParaAmortizacao infoParaAmortizacao, bool maior, bool primeiraAmort) {

            RegistroParcela novaParcela = new RegistroParcela();
            // TODO: aqui a parcela vai receber o . alguma coisa
            float nrParcela = float.Parse(parcelado.nparcelas) + 0.1f;
            novaParcela.nparcelas = nrParcela.ToString();
            novaParcela.dataVencimento = infoParaAmortizacao.data_vencimento;
            novaParcela.dataCalcAmor = primeiraAmort ? infoParaCalculo.formDataCalculo : parcelado.dataVencimento;
            bool v = UService.maiorQue(novaParcela.dataVencimento, novaParcela.dataCalcAmor);
            if (v) novaParcela.vincenda = true;
            
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