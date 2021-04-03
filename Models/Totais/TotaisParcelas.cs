using System;
using System.Globalization;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace calculadora_api.Models

{
    public class TotaisParcelas
    {

        public TotalParcelasVincendas totalParcelasVincendas {get; set;}
        public TotalParcelasVencidas totalParcelasVencidas {get; set;}


        public TotaisParcelas()
        {
            this.totalParcelasVincendas = new TotalParcelasVincendas();
            this.totalParcelasVencidas = new TotalParcelasVencidas();
        }

        public TotaisParcelas(TotalParcelasVincendas totalParcelasVincendas, TotalParcelasVencidas totalParcelasVencidas)
        {
            this.totalParcelasVincendas = totalParcelasVincendas;
            this.totalParcelasVencidas = totalParcelasVencidas;
        }

        protected string formatReal(float value)
        {
            return string.Format(CultureInfo.GetCultureInfo("pt-BR"), "R$ {0:#,###.##}", value);
        }


        public static TotaisParcelas parse(string jsonString) {
            return JsonSerializer.Deserialize<TotaisParcelas>(jsonString);
        }

        public static TotaisParcelas parse(JToken vincendas, JToken vencidas) {
            TotaisParcelas totais = new TotaisParcelas();
            totais.totalParcelasVencidas = TotalParcelasVencidas.parse(vencidas);
            totais.totalParcelasVincendas = TotalParcelasVincendas.parse(vincendas);
            return totais;
        }


        public override string ToString()
        {
            return "TotaisParcelas: ["
            + "\n\t\t vincendas -> " + totalParcelasVincendas
            + "\n\t\t vencidas -> " + totalParcelasVencidas
            + "\n\t]\n\n"
            ;
        }
    }
}