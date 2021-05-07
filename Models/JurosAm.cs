using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models

{
    public class JurosAm
    {

        [Key]
        public int dias { get; set; }
        public float percentsJuros { get; set; }
        public float moneyValue { get; set; }


        public static JurosAm parse(JToken jurosAmJson)
        {
            Console.WriteLine("****************************************");
            var a = jurosAmJson.SelectToken("percentsJuros");
            Console.WriteLine("****************************************");
            Console.WriteLine(a);
            // JurosAm info = jurosAmJson.ToObject<JurosAm>();
            // return info;
            return new JurosAm();
        }

       
        public override string ToString()
        {
            return " Juros AM: ["
            + "\n\t\t\t dias -> " + dias
            + "\n\t\t\t percentsJuros -> " + percentsJuros
            + "\n\t\t\t moneyValue -> " + moneyValue + "\n\t\t ]"
            ;
        }
    }
}