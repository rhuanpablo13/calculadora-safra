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
            EncargosMonetarios em = JsonSerializer.Deserialize<EncargosMonetarios>(jsonString);
            // float x = 270.61864847349717630804948048M;
            // float y = Math.Round(x,3);

            // float d = Math.Round(em.correcaoPeloIndice, 3);
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