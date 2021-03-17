using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{

    public class InfoParaCalculo
    {

        [Key]
        public DateTime formDataCalculo { get; set; }
        public DateTime formUltimaAtualizacao { get; set; }
        public float formMulta { get; set; }
        public float formJuros { get; set; }
        public float formHonorarios { get; set; }
        public float formMultaSobContrato { get; set; }
        public string formIndice { get; set; }
        public float formIndiceEncargos { get; set; }
        public int formDesagio { get; set; }
        public bool isDate { get; set; }

        public float desagio { get; set; }

        public InfoParaCalculo()
        {
            formDataCalculo = new DateTime();
            formUltimaAtualizacao = new DateTime();
            //infoContrato = new InfoContrato();
        }


        public static InfoParaCalculo parse(JToken infoParaCaluloJson)
        {
            InfoParaCalculo info = new InfoParaCalculo();
            // if (infoParaCaluloJson.SelectToken("formUltimaAtualizacao") == null) {
            //     info.formUltimaAtualizacao = new DateTime();
            //     infoParaCaluloJson = JToken.FromObject(info);
            // }
            info = infoParaCaluloJson.ToObject<InfoParaCalculo>();
            return info;
        }


        public override string ToString()
        {
            return "Info Para Calculo: ["
            + "\n\t\t formDataCalculo -> " + formDataCalculo.ToString("yyyy-MM-dd")
            + "\n\t\t formUltimaAtualizacao -> " + formUltimaAtualizacao.ToString("yyyy-MM-dd")
            + "\n\t\t formMulta -> " + formMulta
            + "\n\t\t formJuros -> " + formJuros
            + "\n\t\t formHonorarios -> " + formHonorarios
            + "\n\t\t formMultaSobContrato -> " + formMultaSobContrato
            + "\n\t\t formIndice -> " + formIndice
            + "\n\t\t formIndiceEncargos -> " + formIndiceEncargos
            + "\n\t\t formDesagio -> " + formDesagio
            + "\n\t\t isDate -> " + isDate
            + "\t]\n"
            ;
        }
    }
}