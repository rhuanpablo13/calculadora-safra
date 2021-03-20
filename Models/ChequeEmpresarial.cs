using System;
using System.ComponentModel.DataAnnotations;
using Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{

    // exemplo do json que vai ser recebido
    // {
    //     "table": [],
    //     "contractRef": "SBA.132559/200110900331851 - MUTUO PRE RENEGfalse",
    //     "infoParaCalculo": {
    //         // grupo 2
    //         "formDataCalculo": "2021-02-27",
    //         "formMulta": 2,
    //         "formJuros": 3,
    //         "formHonorarios": 3,
    //         "formMultaSobContrato": 2,
    //         "formIndice": "Encargos Contratuais %",
    //         "formIndiceEncargos": 1
    //     },
    //     "infoLancamento": {
    // "dataVencimento":"2017-07-10",
    // "saldoDevedor": 10000.00,
    // "tipoLancamento":1,
    // "dataBaseAtual": "2017-07-10",
    // "valorLancamento":20.00
    //     }
    // }


    public class ChequeEmpresarial
    {
        [Key]
        public int id { get; set; }
        public DateTime dataBase { get; set; }
        public string indiceDB { get; set; }
        public float indiceDataBase { get; set; }
        public string indiceBA { get; set; }
        public float indiceDataBaseAtual { get; set; }
        public DateTime dataBaseAtual { get; set; }
        public float valorDevedor { get; set; }
        public EncargosMonetarios encargosMonetarios { get; set; }
        public float lancamentos { get; set; }
        public string tipoLancamento { get; set; }
        public float valorDevedorAtualizado { get; set; }
        public string contractRef { get; set; }
        public DateTime ultimaAtualizacao { get; set; }
        public InfoParaCalculo infoParaCalculo { get; set; }


        public ChequeEmpresarial()
        {
            encargosMonetarios = new EncargosMonetarios();
            infoParaCalculo = new InfoParaCalculo();
        }

        public void carregarDadosEntrada(string contractRef, InfoParaCalculo infoParaCalculo, InfoLancamento infoLancamento)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;
            this.indiceDB = infoParaCalculo.formIndice;
            this.indiceBA = infoParaCalculo.formIndice;

            // ultimaAtualizacao = dataVencimento
            this.ultimaAtualizacao = infoLancamento.dataVencimento;
            this.dataBaseAtual = infoLancamento.dataBaseAtual;
            this.valorDevedor = infoLancamento.saldoDevedor;
            this.lancamentos = infoLancamento.valorLancamento;
            this.tipoLancamento = infoLancamento.tipoLancamento;
            this.dataBase = ultimaAtualizacao;
        }

        public bool isEmpty()
        {
            return contractRef == null || contractRef.Equals("") || contractRef.Length == 0;
        }


        public override string ToString()
        {
            return "Cheque Empresarial: ["
            + "\n\t id -> " + id
            + "\n\t dataBase -> " + dataBase.ToString("yyyy-MM-dd")
            + "\n\t indiceDB -> " + indiceDB
            + "\n\t indiceDataBase -> " + indiceDataBase
            + "\n\t indiceBA -> " + indiceBA
            + "\n\t indiceDataBaseAtual -> " + indiceDataBaseAtual
            + "\n\t dataBaseAtual -> " + dataBaseAtual.ToString("yyyy-MM-dd")
            + "\n\t valorDevedor -> " + valorDevedor
            + "\n\t encargosMonetarios -> " + encargosMonetarios
            + "\n\t lancamentos -> " + lancamentos
            + "\n\t tipoLancamento -> " + tipoLancamento
            + "\n\t valorDevedorAtualizado -> " + valorDevedorAtualizado
            + "\n\t contractRef -> " + contractRef
            + "\n\t ultimaAtualizacao -> " + ultimaAtualizacao.ToString("yyyy-MM-dd")
            + "\n\t infoParaCalculo -> " + infoParaCalculo
            + "\n]\n\n"
            ;
        }

    }



}
