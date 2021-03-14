using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class DadosLancamento {
        
        private dynamic dynamic { get; set; }

        public string contractRef { get; set; }
        public string formDataCalculo { get; set; }
        public double formMulta { get; set; }
        public double formJuros { get; set; }
        public double formHonorarios { get; set; }
        public double formMultaSobContrato { get; set; }
        public string formIndice { get; set; }
        public double formIndiceEncargos { get; set; }
        public string dataVencimento { get; set; }
        public double saldoDevedor { get; set; }
        public string tipoLancamento { get; set; }
        public string dataBaseAtual { get; set; }
        public double valorLancamento { get; set; }



        public DadosLancamento(JObject dados) {
            Object tmp = dados.ToObject<Object>();
            dynamic = JsonConvert.DeserializeObject(dados.ToString());
        }


        public DadosLancamento(dynamic dynamic) {
            this.dynamic = dynamic;
        }


        public void parse() {
            contractRef = dynamic["contractRef"];
            formDataCalculo = dynamic["infoParaCalculo"]["formDataCalculo"];
            formMulta = dynamic["infoParaCalculo"]["formMulta"];
            formJuros = dynamic["infoParaCalculo"]["formJuros"];
            formHonorarios = dynamic["infoParaCalculo"]["formHonorarios"];
            formMultaSobContrato = dynamic["infoParaCalculo"]["formMultaSobContrato"];
            formIndice = dynamic["infoParaCalculo"]["formIndice"];
            formIndiceEncargos = dynamic["infoParaCalculo"]["formIndiceEncargos"];
            dataVencimento = dynamic["infoLancamento"]["dataVencimento"];
            saldoDevedor = dynamic["infoLancamento"]["saldoDevedor"];
            tipoLancamento = dynamic["infoLancamento"]["tipoLancamento"];
            dataBaseAtual = dynamic["infoLancamento"]["dataBaseAtual"];
            valorLancamento = dynamic["infoLancamento"]["valorLancamento"];
        }

        public override string ToString() {
            return "RegistroParaCalculo: ["
            + "\n\t formDataCalculo -> " + formDataCalculo
            + "\n\t formMulta -> " + formMulta
            + "\n\t formJuros -> " + formJuros
            + "\n\t formHonorarios -> " + formHonorarios
            + "\n\t formMultaSobContrato -> " + formMultaSobContrato
            + "\n\t formIndice -> " + formIndice
            + "\n\t formIndiceEncargos -> " + formIndiceEncargos
            + "\n\t dataVencimento -> " + dataVencimento
            + "\n\t saldoDevedor -> " + saldoDevedor
            + "\n\t tipoLancamento -> " + tipoLancamento
            + "\n\t dataBaseAtual -> " + dataBaseAtual
            + "\n\t valorLancamento -> " + valorLancamento
            + "\n]\n\n";
        }    
    }
}   