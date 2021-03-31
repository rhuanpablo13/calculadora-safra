using System;
using System.ComponentModel.DataAnnotations;
using calculadora_api.Dao;
// using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace calculadora_api.Models

{

    public class Parcelado
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
        public string tipoParcela { get; set; }
        public InfoParaAmortizacao infoParaAmortizacao { get; set; }
        public Boolean vincenda { set; get; }
        private dynamic dynamic { get; set; }

        public Parcelado(JObject dados)
        {
            Object tmp = dados.ToObject<Object>();
            dynamic = Newtonsoft.Json.JsonConvert.DeserializeObject(dados.ToString());
        }

        public Parcelado() { }

        public void carregarDadosEntrada(string contractRef, InfoParaCalculo infoParaCalculo, Parcela parcela)
        {
            this.contractRef = contractRef;
            this.parcela = parcela;
            this.indiceDCA = infoParaCalculo.formIndice;
            this.indiceDV = infoParaCalculo.formIndice;
            this.dataCalcAmor = infoParaCalculo.formDataCalculo;
        }


        public ParceladoPreDao parseToDao(InfoParaCalculo infoParaCalculo) {
            ParceladoPreDao dao = new ParceladoPreDao();
            dao.Id = this.Id;
            dao.nparcelas = this.parcela.nparcelas.ToString();
            dao.indiceDV = this.indiceDV;
            dao.indiceDataVencimento = this.indiceDataVencimento;
            dao.indiceDCA = this.indiceDCA;
            dao.indiceDataCalcAmor = this.indiceDataCalcAmor;
            dao.dataCalcAmor = this.dataCalcAmor.ToString("yyyy-MM-dd");
            dao.encargosMonetarios = JsonSerializer.Serialize(this.encargosMonetarios);
            dao.subtotal = this.subtotal;
            dao.valorPMTVincenda = this.valorPMTVincenda;
            dao.amortizacao = this.amortizacao;
            dao.totalDevedor = this.totalDevedor;
            dao.contractRef = this.contractRef;
            dao.ultimaAtualizacao = this.ultimaAtualizacao.ToString("yyyy-MM-dd");
            dao.infoParaCalculo = JsonSerializer.Serialize(infoParaCalculo);
            dao.tipoParcela = this.tipoParcela;
            dao.infoParaAmortizacao = JsonSerializer.Serialize(this.infoParaAmortizacao);
            dao.vincenda = this.vincenda;
            return dao;
        }
        

        public override string ToString()
        {
            return " Parcelado: ["
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
                // + "\n\t infoParaCalculo -> " + infoParaCalculo 
                + "\n\t tipoParcela -> " + tipoParcela 
                + "\n\t infoParaAmortizacao -> " + infoParaAmortizacao 
                + "\n\t vincenda -> " + vincenda + "\n]"
            ;
        }
    }


}