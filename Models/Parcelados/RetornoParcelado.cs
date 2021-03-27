using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{

    public class RetornoParcelado : Retorno<Parcelado>
    {
        public List<ITotal> totais {get; set;} = new List<ITotal>();
        public RetornoParcelado(
            string contractRef, 
            TabelaParcelados tabela,
            InfoParaCalculo infoParaCalculo, 
            Totais rodape
        ) : base(contractRef, tabela, infoParaCalculo, rodape){ 
            this.totais.Add(tabela.totalParcelasVencidas);
            this.totais.Add(tabela.totalParcelasVincendas);
        }
    }
}