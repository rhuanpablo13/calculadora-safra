using System;
using System.ComponentModel.DataAnnotations;


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