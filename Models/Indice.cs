using System;
using System.ComponentModel.DataAnnotations;


namespace calculadora_api.Models

{
    public class Indice
    {
        [Key]
        public int Id { get; set; }
        public string indice { get; set; }
        public DateTime data { get; set; }
        public float valor { get; set; }

    }
}