using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{

    public class ParcelaInicial
    {

        [Key]
        public string nparcelas { get; set; }
        public float parcelaInicial { get; set; }
        public DateTime dataVencimento { get; set; }
        public float valorNoVencimento { get; set; }
        public string status { get; set; }


        public static List<ParcelaInicial> parse(JToken infoParcelaJson)
        {
            List<ParcelaInicial> info = infoParcelaJson.ToObject<List<ParcelaInicial>>();
            return info;
        }


        public static ParcelaInicial parse(string jsonString) {
            return JsonSerializer.Deserialize<ParcelaInicial>(jsonString);
        }


        public override string ToString()
        {
            return " ParcelaInicial: ["
            + "\n\t nparcelas -> " + nparcelas
            + "\n\t parcelaInicial -> " + parcelaInicial
            + "\n\t dataVencimento -> " + dataVencimento
            + "\n\t valorNoVencimento -> " + valorNoVencimento
            + "\n\t status -> " + status + "\n\t ]"
            ;
        }
    }
}
