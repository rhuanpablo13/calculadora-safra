using System;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class TotaisRodape
    {

        public TotaisRodape()
        {
        }

        public TotaisRodape(float subtotal, float honorario, float multa, float total)
        {
            this.subtotal = subtotal;
            this.honorario = honorario;
            this.multa = multa;
            this.total = total;
        }

        public TotaisRodape(float subtotal, float honorario, float multa, float total, float amortizacao)
        {
            this.subtotal = subtotal;
            this.honorario = honorario;
            this.multa = multa;
            this.total = total;
            this.amortizacao = amortizacao;
        }

        public string formatReal(float value)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "R$ {0:#,###.##}", value);
        }

        public float subtotal { get; set; }
        public float honorario { get; set; }
        public float multa { get; set; }
        public float total { get; set; }
        public float amortizacao { get; set; }


        public static TotaisRodape parse(JToken totais) {
            // infoParaAmortizacao.Descendants().OfType<JProperty>().Where(attr => attr.Name.Equals("parcela")).ToList().ForEach(attr => attr.Remove());
            TotaisRodape info = totais.ToObject<TotaisRodape>();
            return info;
        }


        public static TotaisRodape parse(string jsonString) {
            return JsonSerializer.Deserialize<TotaisRodape>(jsonString);
        }

        public override string ToString()
        {
            return "TotaisRodape: ["
            + "\n\t\t subtotal -> " + subtotal
            + "\n\t\t amortizacao -> " + amortizacao
            + "\n\t\t honorario -> " + honorario
            + "\n\t\t multa -> " + multa
            + "\n\t\t total -> " + total
            + "\n\t]\n"
            ;
        }
    }
}