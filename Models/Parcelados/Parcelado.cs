using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{

    public class Parcelado : Retorno<RegistroParcela>
    {
        public TotaisParcelas totais {get; set;} = new TotaisParcelas();
        public List<InfoParaAmortizacao> infoParaAmortizacao { get; set; }

        public Parcelado(
            string contractRef, 
            List<RegistroParcela> registros,
            InfoParaCalculo infoParaCalculo, 
            List<InfoParaAmortizacao> infoParaAmortizacao,
            TotaisRodape rodape,
            TotaisParcelas totaisParcelas
        ) : base(contractRef, registros, infoParaCalculo, rodape){
            this.totais = totaisParcelas;
            this.infoParaAmortizacao = infoParaAmortizacao;
        }

        public Parcelado(
            string contractRef, 
            List<RegistroParcela> registros,
            InfoParaCalculo infoParaCalculo, 
            TotaisRodape rodape,
            TotaisParcelas totaisParcelas
        ) : base(contractRef, registros, infoParaCalculo, rodape){
            this.totais = totaisParcelas;
        }

        public Parcelado(){}

        public static Parcelado parse(JToken totaisJson, JToken infoParaCalculoJson, JToken infoParaAmortizacaoJson, JToken tabelaJson, JToken rodapeJson, JToken contractRefJson)
        {
            TotaisParcelas totais = TotaisParcelas.parse(totaisJson.SelectToken("totalParcelasVincendas"), totaisJson.SelectToken("totalParcelasVencidas"));
            InfoParaCalculo infoParaCalculo = InfoParaCalculo.parse(infoParaCalculoJson);
            List<InfoParaAmortizacao> infoParaAmortizacao = InfoParaAmortizacao.parse(infoParaAmortizacaoJson);
            Tabela<RegistroParcela> registros = new Tabela<RegistroParcela>();
            registros.carregarRegistros(tabelaJson); // TODO: validar o retorno do carregar registros
            TotaisRodape rodape = TotaisRodape.parse(rodapeJson);
            string contrato = contractRefJson.ToString();

            Parcelado retorno = new Parcelado(contrato, registros.getRegistros(), infoParaCalculo, infoParaAmortizacao, rodape, totais);
            return retorno;
        }


        public override string ToString()
        {
            return "Retorno: ["
            + "\n\t contractRef -> " + contractRef
            + "\n\t totais -> " + totais.ToString()
            + "\n\t infoParaCalculo -> " + infoParaCalculo.ToString()
            + "\n\t infoParaAmortizacao -> " + printAmortizacao()
            + "\n\t registros -> " + printTabela()
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