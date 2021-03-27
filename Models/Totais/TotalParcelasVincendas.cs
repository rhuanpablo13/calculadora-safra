using Converter;

namespace calculadora_api.Models
{
    public class TotalParcelasVincendas : ITotal
    {
        public float totalDevedor {get; set;}
        public float valorPMTVincenda {get; set;}

        public TotalParcelasVincendas()
        {
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