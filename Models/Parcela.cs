using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{

    public class Parcela
    {

        [Key]
        public int nparcelas { get; set; }
        public float parcelaInicial { get; set; }
        public DateTime dataVencimento { get; set; }
        public float valorNoVencimento { get; set; }
        public string status { get; set; }


        public static List<Parcela> parse(JToken infoParcelaJson)
        {
            List<Parcela> info = infoParcelaJson.ToObject<List<Parcela>>();
            return info;
        }


        public override string ToString()
        {
            return " Parcela: ["
            + "\n\t nparcelas -> " + nparcelas
            + "\n\t parcelaInicial -> " + parcelaInicial
            + "\n\t dataVencimento -> " + dataVencimento
            + "\n\t valorNoVencimento -> " + valorNoVencimento
            + "\n\t status -> " + status + "\n\t ]"
            ;
        }
    }
}