using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    // {
    //   contractRef: 'SBA.132393/20193400197350000 - NÃO DEFINIDOfalse',
    //   table: [],
    //   infoParaCalculo: {
    //     formDataCalculo: '2018-12-01',
    //     formUltimaAtualizacao: '',
    //     formMulta: 10,
    //     formJuros: 10,
    //     formHonorarios: 10,
    //     formMultaSobContrato: 10,
    //     formIndice: 'INPC/IBGE',
    //     formIndiceEncargos: '',
    //     formDesagio: 2,
    //     isDate: false
    //   },
    //   tableParcelas: [
    //     {
    //       nparcelas: 1,
    //       parcelaInicial: 1,
    //       dataVencimento: '2018-01-01',
    //       valorNoVencimento: 1000,
    //       status: 'aberto'
    //     }
    //   ]
    // }


    public class ParceladoPre
    {
        [Key]
        public int Id { get; set; }
        public int nparcelas { get; set; }
        public float parcelaInicial { get; set; }
        public DateTime dataVencimento { get; set; }
        public string indiceDV { get; set; }
        public float indiceDataVencimento { get; set; }
        public string indiceDCA { get; set; }
        public float indiceDataCalcAmor { get; set; }
        public DateTime dataCalcAmor { get; set; }

        public float valorNoVencimento { get; set; }
        public EncargosMonetarios encargosMonetarios { get; set; }

        public float subtotal { get; set; }
        public float valorPMTVincenda { get; set; }
        public float amortizacao { get; set; }
        public float totalDevedor { get; set; }
        public string contractRef { get; set; }
        public string status { get; set; }
        public DateTime ultimaAtualizacao { get; set; }
        public InfoParaCalculo infoParaCalculo { get; set; }
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

        public void popularParceladoPre(string contractRef, InfoParaCalculo infoParaCalculo)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;

            this.nparcelas = nparcelas;
            this.parcelaInicial = parcelaInicial;
            this.dataVencimento = dataVencimento;
            this.valorNoVencimento = valorNoVencimento;
            this.status = status;
        }
    }


}