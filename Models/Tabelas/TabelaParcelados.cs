using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class TabelaParcelados : Tabela<Parcelado>
    {
        public TotalParcelasVincendas totalParcelasVincendas {get; set;}
        public TotalParcelasVencidas totalParcelasVencidas {get; set;}

        public TabelaParcelados() {
            this.totalParcelasVencidas = new TotalParcelasVencidas();
            this.totalParcelasVincendas = new TotalParcelasVincendas();
        }

    }

}