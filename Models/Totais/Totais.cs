using System;
using System.Globalization;

namespace calculadora_api.Models

{
    public class Totais
    {

        public Totais()
        {
        }

        public Totais(float subtotal, float honorario, float multa, float total)
        {
            this.subtotal = formatReal(subtotal);
            this.honorario = formatReal(honorario);
            this.multa = formatReal(multa);
            this.total = formatReal(total);
        }

        protected string formatReal(float value)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "US$ {0:#,###.##}", value);
        }

        public string subtotal { get; set; }
        public string honorario { get; set; }
        public string multa { get; set; }
        public string total { get; set; }

        public void setSubtotal(float subtotal) => this.subtotal = this.formatReal(subtotal);
        public void setHonorario(float honorario) => this.honorario = this.formatReal(honorario);
        public void setMulta(float multa) => this.multa = this.formatReal(multa);
        public void setTotal(float total) => this.total = this.formatReal(total);


        public override string ToString()
        {
            return "Totais: ["
            + "\n\t subtotal -> " + subtotal
            + "\n\t honorario -> " + honorario
            + "\n\t multa -> " + multa
            + "\n\t total -> " + total
            + "\n]\n\n"
            ;
        }
    }
}