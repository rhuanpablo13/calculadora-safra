using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class InfoContrato
    {

        [Key]
        public string pasta { get; set; }
        public string contrato { get; set; }
        public string tipo_contrato { get; set; }
        public string contractRef { get; set; }
        public bool recuperacaoJudicial { get; set; }
        public string cliente { get; set; }
        public string cnpj { get; set; }


        public static InfoContrato parse(JToken infoLancamentoJson)
        {
            InfoContrato info = infoLancamentoJson.ToObject<InfoContrato>();
            return info;
        }


        public override string ToString()
        {
            return "Info Contrato: ["
            + "\n\t\t\t pasta -> " + pasta
            + "\n\t\t\t contrato -> " + contrato
            + "\n\t\t\t tipo_contrato -> " + tipo_contrato
            + "\n\t\t\t contractRef -> " + contractRef
            + "\n\t\t\t recuperacaoJudicial -> " + recuperacaoJudicial
            + "\n\t\t\t cliente -> " + cliente
            + "\n\t\t\t cnpj -> " + cnpj
            + "\n\t\t]\n"
            ;
        }



    }
}