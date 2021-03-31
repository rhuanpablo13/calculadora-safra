using System;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace calculadora_api.Models

{
    public class Totais
    {

        public TotalParcelasVincendas totalParcelasVincendas {get; set;}
        public TotalParcelasVencidas totalParcelasVencidas {get; set;}


        public Totais()
        {
        }

        public Totais(TotalParcelasVincendas totalParcelasVincendas, TotalParcelasVencidas totalParcelasVencidas)
        {
            this.totalParcelasVincendas = totalParcelasVincendas;
            this.totalParcelasVencidas = totalParcelasVencidas;
        }

        protected string formatReal(float value)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "R$ {0:#,###.##}", value);
        }


        public static Totais parse(string jsonString) {
            return JsonSerializer.Deserialize<Totais>(jsonString);
        }

        public static Totais parse(JToken vincendas, JToken vencidas) {
            Totais totais = new Totais();
            totais.totalParcelasVencidas = TotalParcelasVencidas.parse(vencidas);
            totais.totalParcelasVincendas = TotalParcelasVincendas.parse(vincendas);
            return totais;
        }


        public override string ToString()
        {
            return "Totais: ["
            + "\n\t vincendas -> " + totalParcelasVincendas
            + "\n\t vencidas -> " + totalParcelasVencidas
            + "\n]\n\n"
            ;
        }
    }
}