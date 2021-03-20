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

        private readonly IndiceController indiceController;

        public LancamentosService(IndiceController indiceController)
        {
            this.indiceController = indiceController;
        }

        // quando for o primeiro registro
        public ChequeEmpresarial calcular(string contractRef, InfoParaCalculo infoParaCalculo, InfoLancamento infoLancamento)
        {
            ChequeEmpresarial cheque = new ChequeEmpresarial();
            cheque.carregarDadosEntrada(contractRef, infoParaCalculo, infoLancamento);
            return calcular(cheque);
        }

        // quando tiver mais de um registro
        public ChequeEmpresarial calcular(string contractRef, InfoParaCalculo infoParaCalculo, InfoLancamento infoLancamento, ChequeEmpresarial registroSuperior)
        {
            ChequeEmpresarial novoRegistro = new ChequeEmpresarial();
            novoRegistro.carregarDadosEntrada(contractRef, infoParaCalculo, infoLancamento);
            novoRegistro.dataBase = registroSuperior.dataBaseAtual;
            novoRegistro.encargosMonetarios.multa = -1; // multa só é calculada na primeira linha
            novoRegistro.valorDevedor = registroSuperior.valorDevedorAtualizado;
            novoRegistro.indiceBA = infoParaCalculo.formIndice ?? registroSuperior.indiceBA;
            novoRegistro.indiceDB = novoRegistro.indiceBA;
            return calcular(novoRegistro);
        }



        private ChequeEmpresarial calcular(ChequeEmpresarial ce)
        {
            //dias
            ce.encargosMonetarios.jurosAm.dias = numberOfDays(ce.dataBase, ce.dataBaseAtual);

            //indiceDataBase
            ce.indiceDataBase = getIndiceDataBase(ce.indiceDB, ce.dataBase, ce.infoParaCalculo.formIndiceEncargos);

            //indiceDataBaseAtual
            ce.indiceDataBaseAtual = getIndiceDataBase(ce.indiceBA, ce.dataBaseAtual, ce.infoParaCalculo.formIndiceEncargos);

            //correcaoPeloIndice
            ce.encargosMonetarios.correcaoPeloIndice = calcCorrecaoPeloIndice(ce);

            //percentsJuros
            ce.encargosMonetarios.jurosAm.percentsJuros = calcPercentsJuros(ce);

            //moneyValue
            ce.encargosMonetarios.jurosAm.moneyValue = calcMoneyValue(ce);

            // -1 indica que é a primeira linha sendo inserida
            ce.encargosMonetarios.multa = calcMulta(ce);

            //valorDevedorAtualizado
            ce.valorDevedorAtualizado = calcValorDevedorAtualizado(ce);

            return ce;
        }



        private float calcCorrecaoPeloIndice(ChequeEmpresarial ce)
        {
            if (ce.indiceDB == "Encargos Contratuais %")
                return ((ce.valorDevedor * (ce.indiceDataBaseAtual / 100)) / 30) * ce.encargosMonetarios.jurosAm.dias;
            else
                return (ce.valorDevedor / ce.indiceDataBase) * ce.indiceDataBaseAtual - ce.valorDevedor;
        }

        private float calcPercentsJuros(ChequeEmpresarial ce)
        {
            return (ce.infoParaCalculo.formJuros / 30) * ce.encargosMonetarios.jurosAm.dias;
        }

        private float calcMoneyValue(ChequeEmpresarial ce)
        {
            return ((ce.valorDevedor + ce.encargosMonetarios.correcaoPeloIndice) / 30) * ce.encargosMonetarios.jurosAm.dias * (ce.infoParaCalculo.formJuros / 100);
        }

        private float calcMulta(ChequeEmpresarial ce)
        {
            if (ce.encargosMonetarios.multa != -1)
                return (ce.valorDevedor + ce.encargosMonetarios.correcaoPeloIndice + ce.encargosMonetarios.jurosAm.moneyValue) * (ce.infoParaCalculo.formMulta / 100);
            return -1;
        }



        private float calcValorDevedorAtualizado(ChequeEmpresarial ce)
        {
            if (ce.tipoLancamento == "debit")
                return ce.valorDevedor + ce.encargosMonetarios.correcaoPeloIndice + ce.encargosMonetarios.jurosAm.moneyValue + ce.encargosMonetarios.multa + ce.lancamentos;
            else
                return (ce.valorDevedor + ce.encargosMonetarios.correcaoPeloIndice) - ((ce.lancamentos - ce.encargosMonetarios.multa) - ce.encargosMonetarios.jurosAm.moneyValue);
        }


        public Totais calcularTotais(Tabela<ChequeEmpresarial> table)
        {

            float subtotal = 0;
            float honorarios = 0;
            float multa = 0;
            float total = 0;

            if (!table.temRegistros())
            {
                return null;
            }

            ChequeEmpresarial cb = table.getUltimoRegistro();
            subtotal = cb.valorDevedorAtualizado;

            // honorarios = valorDevedorAtualizado 
            honorarios = subtotal * (cb.infoParaCalculo.formHonorarios / 100);
            // multa = ((valorDevedorAtualizado + honorarios) * multa_sob_contrato grupo 2 / 100
            multa = (subtotal + honorarios) * (cb.infoParaCalculo.formMultaSobContrato / 100);
            // total_grandtotal = tmulta_sob_contrato + honorarios + valorDevedorAtualizado;
            total = honorarios + subtotal + multa;

            return new Totais(subtotal, honorarios, multa, total);
        }





        private float getIndiceDataBase(string indice, DateTime date, float formIndiceEncargos)
        {
            return indiceController.getIndice(
                indice,
                date,
                formIndiceEncargos
            );
        }

        private int numberOfDays(DateTime minor, DateTime major)
        {
            int days = major.Subtract(minor).Days;
            return days >= 0 ? days : minor.Subtract(major).Days;
        }
    }
}