using Converter;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class TotalParcelasVincendas : ITotal
    {
        public float totalDevedor {get; set;}
        public float valorPMTVincenda {get; set;}

        public TotalParcelasVincendas()
        {
        }

        public static TotalParcelasVincendas parse(JToken infoParcelaJson)
        {
            TotalParcelasVincendas info = infoParcelaJson.ToObject<TotalParcelasVincendas>();
            return info;
        }
        
        public string toJson() {
            return U.toStringJson(this);
        }

        public override string ToString()
        {
            return "TotalParcelasVincendas: ["
            + "\n\t totalDevedor -> " + totalDevedor
            + "\n\t valorPMTVincenda -> " + valorPMTVincenda
            + "\n]\n"
            ;
        }
    }
}