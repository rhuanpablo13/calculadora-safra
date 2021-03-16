using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class InfoParaCalculo
    {

        [Key]
        public DateTime formDataCalculo { get; set; }
        public float formMulta { get; set; }
        public float formJuros { get; set; }
        public float formHonorarios { get; set; }
        public float formMultaSobContrato { get; set; }
        public string formIndice { get; set; }
        public float formIndiceEncargos { get; set; }
        public bool isDate { get; set; }
        //public InfoContrato infoContrato { get; set; }

        public float desagio { get; set; }

        public InfoParaCalculo()
        {
            //infoContrato = new InfoContrato();
        }


        public static InfoParaCalculo parse(JToken infoParaCaluloJson)
        {
            InfoParaCalculo info = infoParaCaluloJson.ToObject<InfoParaCalculo>();
            return info;
        }


        public override string ToString()
        {
            return "Info Para Calculo: ["
            + "\n\t\t formDataCalculo -> " + formDataCalculo.ToString("yyyy-MM-dd")
            + "\n\t\t formMulta -> " + formMulta
            + "\n\t\t formJuros -> " + formJuros
            + "\n\t\t formHonorarios -> " + formHonorarios
            + "\n\t\t formMultaSobContrato -> " + formMultaSobContrato
            + "\n\t\t formIndice -> " + formIndice
            + "\n\t\t formIndiceEncargos -> " + formIndiceEncargos
            + "\n\t\t isDate -> " + isDate
            + "\t]\n"
            ;
        }
    }
}