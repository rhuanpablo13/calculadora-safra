using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class TabelaParcelados : Tabela<Parcelado>
    {

        public TabelaParcelados() {
        }

        public static TabelaParcelados parse(JToken tabelaParceladosJson)
        {
            TabelaParcelados tabela = new TabelaParcelados();
            tabela.carregarRegistros(tabelaParceladosJson);
            return tabela;
        }

        public static TabelaParcelados parse(string jsonString) {
            return JsonSerializer.Deserialize<TabelaParcelados>(jsonString);
        }


    }

}