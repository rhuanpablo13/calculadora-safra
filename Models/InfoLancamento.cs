using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using Converter;
using Newtonsoft.Json;

namespace calculadora_api.Models
{
    public class InfoLancamento
    {

        [Key]
        public DateTime dataVencimento { get; set; }
        public float saldoDevedor { get; set; }
        public string tipoLancamento { get; set; }
        public DateTime dataBaseAtual { get; set; }
        public float valorLancamento { get; set; }

        public float desagio { get; set; }


        public InfoLancamento() { }


        public static InfoLancamento parse(JToken infoLancamentoJson)
        {
            InfoLancamento info = infoLancamentoJson.ToObject<InfoLancamento>();
            return info;
        }

        public override string ToString()
        {
            return "Info Lançamento: ["
            + "\n\t\t dataVencimento -> " + dataVencimento
            + "\n\t\t saldoDevedor -> " + saldoDevedor
            + "\n\t\t tipoLancamento -> " + tipoLancamento
            + "\n\t\t dataBaseAtual -> " + dataBaseAtual
            + "\n\t\t valorLancamento -> " + valorLancamento
            + "\t]\n"
            ;
        }
    }
}