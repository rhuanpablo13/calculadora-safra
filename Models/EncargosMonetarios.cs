using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System;

namespace calculadora_api.Models

{
    public class EncargosMonetarios
    {

        [Key]
        public float correcaoPeloIndice { get; set; }
        public JurosAm jurosAm { get; set; }
        public float multa { get; set; }

        public EncargosMonetarios()
        {
            jurosAm = new JurosAm();
        }

        public static EncargosMonetarios parse(string jsonString) {
            Console.WriteLine(jsonString);
            string n = jsonString
                .Split("\"percentsJuros\"")[1]
                .Split("\"moneyValue\"")[0]
                .Replace("\"", "")
                .Replace(":", "")
                .Replace(",", "");

            jsonString = jsonString.Replace("\"percentsJuros\"" + ":" + "\"" + n + "\"", "\"percentsJuros\"" + ":" + n);
            jsonString = jsonString.Replace("\"NaN\"", "0");
            EncargosMonetarios em = JsonSerializer.Deserialize<EncargosMonetarios>(jsonString);
            return em;
        }

        public override string ToString()
        {
            return "Encargos Monetarios: ["
            + "\n\t\t correcaoPeloIndice -> " + correcaoPeloIndice
            + "\n\t\t jurosAm -> " + jurosAm
            + "\n\t\t multa -> " + multa
            + "\n\t ]"
            ;
        }
    }
}