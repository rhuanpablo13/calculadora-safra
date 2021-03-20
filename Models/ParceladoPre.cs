using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{

    public class ParceladoPre
    {
        [Key]
        public int Id { get; set; }
        public Parcela parcela = new Parcela(); 
        public string indiceDV { get; set; }
        public float indiceDataVencimento { get; set; }
        public string indiceDCA { get; set; }
        public float indiceDataCalcAmor { get; set; }
        public DateTime dataCalcAmor { get; set; }

        
        public EncargosMonetarios encargosMonetarios = new EncargosMonetarios();

        public float subtotal { get; set; }
        public float valorPMTVincenda { get; set; }
        public float amortizacao { get; set; }
        public float totalDevedor { get; set; }
        public string contractRef { get; set; }
        
        public DateTime ultimaAtualizacao { get; set; }
        public InfoParaCalculo infoParaCalculo = new InfoParaCalculo();
        public string tipoParcela { get; set; }
        public string infoParaAmortizacao { get; set; }
        public Boolean vincenda { set; get; }
        private dynamic dynamic { get; set; }

        public ParceladoPre(JObject dados)
        {
            Object tmp = dados.ToObject<Object>();
            dynamic = JsonConvert.DeserializeObject(dados.ToString());
        }

        public ParceladoPre() { }

        public void carregarDadosEntrada(string contractRef, InfoParaCalculo infoParaCalculo, Parcela parcela)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;
            this.parcela = parcela;
            this.indiceDCA = infoParaCalculo.formIndice;
            this.indiceDV = infoParaCalculo.formIndice;

            this.dataCalcAmor = parcela.dataVencimento; //revisar
        }
        

        public override string ToString()
        {
            return " ParceladoPre: ["
                + "\n\t Id -> " + Id 
                + "\n\t parcela -> " + parcela 
                + "\n\t indiceDV -> " + indiceDV 
                + "\n\t indiceDataVencimento -> " + indiceDataVencimento 
                + "\n\t indiceDCA -> " + indiceDCA 
                + "\n\t indiceDataCalcAmor -> " + indiceDataCalcAmor 
                + "\n\t dataCalcAmor -> " + dataCalcAmor 
                + "\n\t encargosMonetarios -> " + encargosMonetarios 
                + "\n\t subtotal -> " + subtotal 
                + "\n\t valorPMTVincenda -> " + valorPMTVincenda 
                + "\n\t amortizacao -> " + amortizacao 
                + "\n\t totalDevedor -> " + totalDevedor 
                + "\n\t contractRef -> " + contractRef 
                + "\n\t ultimaAtualizacao -> " + ultimaAtualizacao 
                + "\n\t infoParaCalculo -> " + infoParaCalculo 
                + "\n\t tipoParcela -> " + tipoParcela 
                + "\n\t infoParaAmortizacao -> " + infoParaAmortizacao 
                + "\n\t vincenda -> " + vincenda + "\n]"
            ;
        }
    }


}