using System.ComponentModel.DataAnnotations;
using System.Text.Json;
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
            return JsonSerializer.Deserialize<EncargosMonetarios>(jsonString);
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