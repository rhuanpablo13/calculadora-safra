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
        public List<T> tabela { get; set; }
        public Rodape rodape { get; set; }


        public Retorno() {}
        
        public Retorno(string contractRef, Tabela<T> tabela, InfoParaCalculo infoParaCalculo, Rodape rodape)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;
            this.tabela = tabela.getRegistros();
            this.rodape = rodape;
        }

        public Retorno(string contractRef, Tabela<T> tabela, Rodape rodape)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = new InfoParaCalculo();
            this.tabela = tabela.getRegistros();
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
            if (tabela.Count == 0) return "[]";

            string temp = "[\n\t";
            foreach (var item in tabela)
            {
                temp += item.ToString() + "\n\t";
            }
            return temp += "]";
        }
    }
}