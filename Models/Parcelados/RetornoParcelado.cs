using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{

    public class RetornoParcelado : Retorno<Parcelado>
    {
        public TotaisParcelas totais {get; set;} = new TotaisParcelas();
        public List<InfoParaAmortizacao> infoParaAmortizacao { get; set; }

        public RetornoParcelado(
            string contractRef, 
            TabelaParcelados tabela,
            InfoParaCalculo infoParaCalculo, 
            List<InfoParaAmortizacao> infoParaAmortizacao,
            TotaisRodape rodape,
            TotaisParcelas totaisParcelas
        ) : base(contractRef, tabela, infoParaCalculo, rodape){
            this.totais = totaisParcelas;
            this.infoParaAmortizacao = infoParaAmortizacao;
        }

        public RetornoParcelado(
            string contractRef, 
            TabelaParcelados tabela,
            InfoParaCalculo infoParaCalculo, 
            TotaisRodape rodape,
            TotaisParcelas totaisParcelas
        ) : base(contractRef, tabela, infoParaCalculo, rodape){
            this.totais = totaisParcelas;
        }

        public RetornoParcelado(){}

        public static RetornoParcelado parse(JToken totaisJson, JToken infoParaCalculoJson, JToken infoParaAmortizacaoJson, JToken tabelaJson, JToken rodapeJson, JToken contractRefJson)
        {
            TotaisParcelas totais = TotaisParcelas.parse(totaisJson.SelectToken("totalParcelasVincendas"), totaisJson.SelectToken("totalParcelasVencidas"));
            InfoParaCalculo infoParaCalculo = InfoParaCalculo.parse(infoParaCalculoJson);
            List<InfoParaAmortizacao> infoParaAmortizacao = InfoParaAmortizacao.parse(infoParaAmortizacaoJson);
            TabelaParcelados tabela = TabelaParcelados.parse(tabelaJson);
            TotaisRodape rodape = TotaisRodape.parse(rodapeJson);
            string contrato = contractRefJson.ToString();

            RetornoParcelado retorno = new RetornoParcelado(contrato, tabela, infoParaCalculo, infoParaAmortizacao, rodape, totais);
            return retorno;
        }


        public override string ToString()
        {
            return "Retorno: ["
            + "\n\t contractRef -> " + contractRef
            + "\n\t totais -> " + totais.ToString()
            + "\n\t infoParaCalculo -> " + infoParaCalculo.ToString()
            + "\n\t infoParaAmortizacao -> " + printAmortizacao()
            + "\n\t tabela -> " + printTabela()
            + "\n\t rodape -> " + rodape.ToString()
            + "\n]\n\n"
            ;
        }

        private string printAmortizacao() {
            if (infoParaAmortizacao == null || infoParaAmortizacao.Count == 0) return "[]";
            string str = "";
            infoParaAmortizacao.ForEach(info => str += info.ToString());
            return str;
        }
        
    }
}