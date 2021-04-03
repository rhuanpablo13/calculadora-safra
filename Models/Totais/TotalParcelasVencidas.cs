using System.Globalization;
using Converter;
using Newtonsoft.Json.Linq;
using System;

namespace calculadora_api.Models

{
    public class TotalParcelasVencidas : ITotal
    {
        public float valorNoVencimento {get; set;}
        public float correcaoPeloIndice {get; set;}
        public float money {get; set;}
        public float somaMulta {get; set;}
        public float subTotal {get; set;}
        public float amortizacao {get; set;}
        public float totalDevedor {get; set;}

        public TotalParcelasVencidas()
        {
        }

        public static TotalParcelasVencidas parse(JToken infoParcelaJson)
        {
            TotalParcelasVencidas info = infoParcelaJson.ToObject<TotalParcelasVencidas>();
            return info;
        }

        public string toJson() {
            return U.toStringJson(this);
        }

         public override string ToString()
        {
            return "TotalParcelasVencidas: ["
            + "\n\t\t\t valorNoVencimento -> " + valorNoVencimento
            + "\n\t\t\t correcaoPeloIndice -> " + correcaoPeloIndice
            + "\n\t\t\t money -> " + money
            + "\n\t\t\t somaMulta -> " + somaMulta
            + "\n\t\t\t subTotal -> " + subTotal
            + "\n\t\t\t amortizacao -> " + amortizacao
            + "\n\t\t\t totalDevedor -> " + totalDevedor
            + "\n\t\t]\n"
            ;
        }

    }
}