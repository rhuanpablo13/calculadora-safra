using System;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class Rodape
    {

        public Rodape()
        {
        }

        public Rodape(float subtotal, float honorario, float multa, float total)
        {
            this.subtotal = formatReal(subtotal);
            this.honorario = formatReal(honorario);
            this.multa = formatReal(multa);
            this.total = formatReal(total);
        }

        public Rodape(float subtotal, float honorario, float multa, float total, float amortizacao)
        {
            this.subtotal = formatReal(subtotal);
            this.honorario = formatReal(honorario);
            this.multa = formatReal(multa);
            this.total = formatReal(total);
            this.amortizacao = formatReal(amortizacao);
        }

        protected string formatReal(float value)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "US$ {0:#,###.##}", value);
        }

        public string subtotal { get; set; }
        public string honorario { get; set; }
        public string multa { get; set; }
        public string total { get; set; }
        public string amortizacao { get; set; }

        public void setSubtotal(float subtotal) => this.subtotal = this.formatReal(subtotal);
        public void setHonorario(float honorario) => this.honorario = this.formatReal(honorario);
        public void setMulta(float multa) => this.multa = this.formatReal(multa);
        public void setTotal(float total) => this.total = this.formatReal(total);


        public static Rodape parse(JToken totais) {
            // infoParaAmortizacao.Descendants().OfType<JProperty>().Where(attr => attr.Name.Equals("parcela")).ToList().ForEach(attr => attr.Remove());
            totais.ToString().Replace("US$ ", "");
            Rodape info = totais.ToObject<Rodape>();
            return info;
        }


        public static Rodape parse(string jsonString) {
            return JsonSerializer.Deserialize<Rodape>(jsonString);
        }

        public override string ToString()
        {
            return "Rodape: ["
            + "\n\t subtotal -> " + subtotal
            + "\n\t amortizacao -> " + amortizacao
            + "\n\t honorario -> " + honorario
            + "\n\t multa -> " + multa
            + "\n\t total -> " + total
            + "\n]\n\n"
            ;
        }
    }
}