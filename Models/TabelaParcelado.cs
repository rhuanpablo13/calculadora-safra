using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class TabelaParcelado
    {

        private List<ParceladoPre> registros { get; set; }


        public TabelaParcelado()
        {
            registros = new List<ParceladoPre>();
        }



        public void carregarRegistrosTabela(JObject dados)
        {
            var temp = dados.SelectToken("table");
            if (temp != null)
            {
                this.registros.Clear();
                this.registros = temp.ToObject<List<ParceladoPre>>();
            }
        }


        // criar model para table parcelas
        // nrParcelas
        // dtVencimento
        // vlNoVencimento
        // status
        public void carregarRegistrosParcelas(JObject dados)
        {
            var temp = dados.SelectToken("tableParcelas");
            if (temp != null)
            {
                this.registros.Clear();
                this.registros = temp.ToObject<List<ParceladoPre>>();
            }
        }


        public bool temRegistros()
        {
            return registros.Count > 0;
        }



        public override string ToString()
        {
            string str = "";
            if (registros.Count > 0)
            {
                foreach (ParceladoPre item in registros)
                {
                    str += item.ToString() + "\n";
                }
            }
            return str;
        }
    }

}