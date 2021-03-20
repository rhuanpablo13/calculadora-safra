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

        public ParceladoPreService(IndiceController indiceController)
        {
            this.indiceController = indiceController;
        }


        public List<ParceladoPre> calcular(string contractRef, InfoParaCalculo infoParaCalculo, List<Parcela> parcelas)
        {
            ParceladoPre parcelado = new ParceladoPre();
            List<ParceladoPre> ls = new List<ParceladoPre>();
            foreach (var parcela in parcelas)
            {
                parcelado.carregarDadosEntrada(contractRef, infoParaCalculo, parcela);
                parcelado = calcular(parcelado);
                ls.Add(parcelado);
            }
            return ls;
        }



        // public Tabela calcular(DadosParceladoPre dadosLancamento, Tabela table)
        // {
        //     // ParceladoPre novoRegistro = new ParceladoPre();
        //     // novoRegistro.copyFromDadosLancamento(dadosLancamento);
        //     // novoRegistro.dataBase = registroSuperior.dataBaseAtual;
        //     // novoRegistro.encargosMonetarios.multa = -1; // multa só é calculada na primeira linha
        //     // novoRegistro.valorDevedor = registroSuperior.valorDevedorAtualizado;
        //     // novoRegistro.indiceBA = dadosLancamento.formIndice == null ? registroSuperior.indiceBA : dadosLancamento.formIndice;
        //     // novoRegistro.indiceDB = novoRegistro.indiceBA;
        //     // table.adicionarRegistro(calcular(novoRegistro));
        //     return table;
        // }



        private ParceladoPre calcular(ParceladoPre p)
        {
            InfoParaCalculo ipc = p.infoParaCalculo;
            
            //dias
            p.encargosMonetarios.jurosAm.dias = UService.numberOfDays(p.dataCalcAmor, p.parcela.dataVencimento);
            //indiceDataVencimento
            p.indiceDataVencimento = UService.getIndice(
                p.indiceDV, p.parcela.dataVencimento, ipc.formIndiceEncargos, indiceController
            );
            //indiceDataCalcAmor
            p.indiceDataCalcAmor = UService.getIndice(
                p.indiceDCA, p.dataCalcAmor, ipc.formIndiceEncargos, indiceController
            );

            //correcaoPeloIndice
            p.encargosMonetarios.correcaoPeloIndice = (
                (p.parcela.valorNoVencimento / p.indiceDataVencimento) * p.indiceDataCalcAmor
            ) - p.parcela.valorNoVencimento;
            //percentsJuros
            p.encargosMonetarios.jurosAm.percentsJuros = (p.infoParaCalculo.formJuros / 30) * p.encargosMonetarios.jurosAm.dias;
            //moneyValue
            p.encargosMonetarios.jurosAm.moneyValue = ((p.parcela.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice) / 30) * p.encargosMonetarios.jurosAm.dias * (p.infoParaCalculo.formJuros / 100);
            
            //multa
            p.encargosMonetarios.multa = (p.parcela.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + (p.infoParaCalculo.formMulta / 100));
            //subtotal
            p.subtotal = p.parcela.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa;
            //valorPMTVincenda
            p.valorPMTVincenda = p.parcela.valorNoVencimento * p.infoParaCalculo.desagio;
            
            p.amortizacao = -1;

            //totalDevedor
            p.totalDevedor = p.vincenda 
            ? p.parcela.valorNoVencimento + p.encargosMonetarios.correcaoPeloIndice + p.encargosMonetarios.jurosAm.moneyValue + p.encargosMonetarios.multa + p.amortizacao 
            : p.valorPMTVincenda;

            p.infoParaCalculo = ipc;

            // if (p.vincenda) {
            //     totalParcelasVencidas = [
            //         valorNoVencimento += p.valorNoVencimento,
            //         correcaoPeloIndice += p.correcaoPeloIndice,
            //         money += money,
            //         multa += multa,
            //         subTotal += subTotal,
            //         amortizacao += amortizacao,
            //         totalDevedor += totalDevedor
            //     ];
            // }

            // } else {
            //     totalParcelasVincendas = [
            //         valorPMTVincenda += valorPMTVincenda,
            //         totalDevedor += totalDevedor
            //     ];
            // }


            // Console.WriteLine("*********** dados calculados ***************");
            // Console.WriteLine(p.ToString());

            return p;
        }


        public Totais calcularTotais(Tabela<ParceladoPre> table)
        {

            float subtotal = 0;
            float honorarios = 0;
            float multa = 0;
            float total = 0;

            if (!table.temRegistros())
            {
                return null;
            }

            ParceladoPre cb = table.getUltimoRegistro();
            // subtotal = cb.valorDevedorAtualizado;

            // honorarios = valorDevedorAtualizado 
            honorarios = subtotal * (cb.infoParaCalculo.formHonorarios / 100);
            // multa = ((valorDevedorAtualizado + honorarios) * multa_sob_contrato grupo 2 / 100
            multa = (subtotal + honorarios) * (cb.infoParaCalculo.formMultaSobContrato / 100);
            // total_grandtotal = tmulta_sob_contrato + honorarios + valorDevedorAtualizado;
            total = honorarios + subtotal + multa;

            return new Totais(subtotal, honorarios, multa, total);
        }
    }
}