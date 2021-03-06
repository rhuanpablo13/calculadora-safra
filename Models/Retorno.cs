using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace calculadora_api.Models

{

    public class Retorno
    {        
        public Retorno(string contractRef, Tabela tabela, Totais rodape) {
            this.contractRef = contractRef;
            this.tabela = tabela;
            this.rodape = rodape;
        }

        public string contractRef { get; set; }
        public Tabela tabela { get; set; }
        public Totais rodape { get; set; }


        public override string ToString() {
            return "Retorno: ["
            + "\n\t contractRef -> " + contractRef
            + "\n\t tabela -> " + tabela
            + "\n\t rodape -> " + rodape.ToString()
            + "\n]\n\n"
            ;
        }

    }

    
    public class Totais
    {
        public Totais(double subtotal, double honorario, double multa, double total) {
            this.subtotal = formatReal(subtotal);
            this.honorario = formatReal(honorario);
            this.multa = formatReal(multa);
            this.total = formatReal(total);
        }

        private string formatReal(double value) {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "US$ {0:#,###.##}", value);
        }

        public string subtotal { get; set; }
        public string honorario { get; set; }
        public string multa { get; set; }
        public string total { get; set; }

        public override string ToString() {
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