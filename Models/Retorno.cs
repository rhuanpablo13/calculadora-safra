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
        public Totais rodape { get; set; }


        public Retorno(string contractRef, Tabela<T> tabela, InfoParaCalculo infoParaCalculo, Totais rodape)
        {
            this.contractRef = contractRef;
            this.infoParaCalculo = infoParaCalculo;
            this.tabela = tabela.getRegistros();
            this.rodape = rodape;
        }

        public Retorno(string contractRef, Tabela<T> tabela, Totais rodape)
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
            + "\n\t tabela -> " + tabela.ToString()
            + "\n\t rodape -> " + rodape.ToString()
            + "\n]\n\n"
            ;
        }
    }
}