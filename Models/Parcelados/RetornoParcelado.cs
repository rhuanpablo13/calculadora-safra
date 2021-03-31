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
        public Totais totais {get; set;} = new Totais();

        public RetornoParcelado(
            string contractRef, 
            TabelaParcelados tabela,
            InfoParaCalculo infoParaCalculo, 
            Rodape rodape
        ) : base(contractRef, tabela, infoParaCalculo, rodape){ 
            // this.totais.totalParcelasVencidas = tabela.totalParcelasVencidas;
            // this.totais.totalParcelasVincendas = tabela.totalParcelasVincendas;
        }

        public RetornoParcelado(){}

        public static RetornoParcelado parse(JToken totais, JToken infoParaCalculo, JToken tabela, JToken rodape, JToken contractRef)
        {
            Totais totais1 = Totais.parse(totais.SelectToken("totalParcelasVincendas"), totais.SelectToken("totalParcelasVencidas"));
            InfoParaCalculo infoParaCalculo1 = InfoParaCalculo.parse(infoParaCalculo);
            TabelaParcelados tabela1 = TabelaParcelados.parse(tabela);
            Rodape rodape1 = Rodape.parse(rodape);
            string cont = contractRef.ToString();

            RetornoParcelado retorno = new RetornoParcelado(cont, tabela1, infoParaCalculo1, rodape1);
            retorno.totais = totais1;
            return retorno;
        }


        public override string ToString()
        {
            return "Retorno: ["
            + "\n\t contractRef -> " + contractRef
            + "\n\t totais -> " + totais.ToString()
            + "\n\t infoParaCalculo -> " + infoParaCalculo.ToString()
            + "\n\t tabela -> " + printTabela()
            + "\n\t rodape -> " + rodape.ToString()
            + "\n]\n\n"
            ;
        }
        
    }
}