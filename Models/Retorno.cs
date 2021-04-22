using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{

    public class Retorno<T>
    {
        public string contractRef { get; set; }
        public InfoParaCalculo infoParaCalculo { get; set; }
        public List<T> tabela { get; set; } // List para que o jsonSerialize saiba transformar em json
        public TotaisRodape rodape { get; set; }


        public Retorno() {}
        
        public Retorno(string contractRef, List<T> tabela, InfoParaCalculo infoParaCalculo, TotaisRodape rodape)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;
            this.tabela = tabela;
            this.rodape = rodape;
        }

        public Retorno(string contractRef, List<T> tabela, TotaisRodape rodape)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = new InfoParaCalculo();
            this.tabela = tabela;
            this.rodape = rodape;
        }

        public override string ToString()
        {
            return "Retorno: ["
            + "\n\t contractRef -> " + contractRef
            + "\n\t infoParaCalculo -> " + infoParaCalculo.ToString()
            + "\n\t tabela -> " + printTabela()
            + "\n\t rodape -> " + rodape.ToString()
            + "\n]\n\n"
            ;
        }

        protected string printTabela() {
            if (tabela.Count == 0) return "\n\t[]";

            string temp = "[\n";
            foreach (var item in tabela)
            {
                temp += "\t" + item.ToString() + "\n";
            }
            return temp += "\t\t]\n";
        }
    }
}