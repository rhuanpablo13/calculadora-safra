using System;
using System.ComponentModel.DataAnnotations;
using calculadora_api.Models;
using Converter;

namespace calculadora_api.Dao

{
    public class ParceladoPreDao
    {
        [Key]
        public int Id { get; set; }
        public string nparcelas { get; set; }
        public float parcelaInicial { get; set; }
        public float indiceDataVencimento { get; set; }
        public float indiceDataCalcAmor { get; set; }
        public float valorNoVencimento { get; set; }
        public float subtotal { get; set; }
        public float amortizacao { get; set; }
        public float totalDevedor { get; set; }
        public string contractRef { get; set; }

        public string dataVencimento { get; set; }
        public string indiceDV { get; set; }
        public string indiceDCA { get; set; }
        public string dataCalcAmor { get; set; }
        public float valorPMTVincenda { get; set; }
        public string status { get; set; }
        public string ultimaAtualizacao { get; set; }
        public string encargosMonetarios { get; set; }
        public string infoParaCalculo { get; set; }
        public string tipoParcela { get; set; }
        public string infoParaAmortizacao { get; set; }
        public Boolean vincenda { set; get; }



        public RegistroParcela parseFrom() {
            RegistroParcela p = new RegistroParcela();
            p.Id = this.Id;
            p.nparcelas = this.nparcelas;
            p.parcelaInicial = this.parcelaInicial;
            p.indiceDataVencimento = this.indiceDataVencimento;
            p.indiceDataCalcAmor = this.indiceDataCalcAmor;
            p.valorNoVencimento = this.valorNoVencimento;
            p.subtotal = this.subtotal;
            p.amortizacao = this.amortizacao;
            p.totalDevedor = this.totalDevedor;
            p.contractRef = this.contractRef;
            p.dataVencimento = U.toDateTime(this.dataVencimento);
            p.indiceDV = this.indiceDV;
            p.indiceDCA = this.indiceDCA;
            p.dataCalcAmor = U.toDateTime(this.dataCalcAmor);
            p.valorPMTVincenda = this.valorPMTVincenda;
            p.status = this.status;
            p.ultimaAtualizacao = U.toDateTime(this.ultimaAtualizacao);
            p.encargosMonetarios = EncargosMonetarios.parse(this.encargosMonetarios);
            p.tipoParcela = this.tipoParcela;
            //p.infoParaAmortizacao = InfoParaAmortizacao.parse(this.infoParaAmortizacao);
            p.vincenda = this.vincenda;
            return p;
        }


        public InfoParaCalculo GetInfoParaCalculo() {
            return InfoParaCalculo.parse(this.infoParaCalculo);
        }
    }

}