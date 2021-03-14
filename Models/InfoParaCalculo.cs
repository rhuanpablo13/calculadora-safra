using System;
using System.ComponentModel.DataAnnotations;


namespace calculadora_api.Models

{
    public class InfoParaCalculo    {
        public DateTime formDataCalculo { get; set; } 
        public double formMulta { get; set; } 
        public double formJuros { get; set; } 
        public double formHonorarios { get; set; } 
        public double formMultaSobContrato { get; set; } 
        public string formIndice { get; set; } 
        public double formIndiceEncargos { get; set; } 
        public bool isDate { get; set; } 
        public InfoContrato infoContrato { get; set; } 

        public double desagio {get; set;}

        public InfoParaCalculo() {
            infoContrato = new InfoContrato();
        }

        public override string ToString() {
            return "Info Para Calculo: ["
            + "\n\t\t formDataCalculo -> " + formDataCalculo
            + "\n\t\t formMulta -> " + formMulta
            + "\n\t\t formJuros -> " + formJuros
            + "\n\t\t formHonorarios -> " + formHonorarios
            + "\n\t\t formMultaSobContrato -> " + formMultaSobContrato
            + "\n\t\t formIndice -> " + formIndice
            + "\n\t\t formIndiceEncargos -> " + formIndiceEncargos
            + "\n\t\t isDate -> " + isDate
            + "\n\t\t infoContrato -> " + infoContrato
            + "\t]\n"
            ;
        }
    }
}   