using System;
using System.ComponentModel.DataAnnotations;

namespace calculadora_api.Models

{
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public DateTime data { get; set; }
        public string usuario { get; set; }
        public string pasta { get; set; }
        public string contrato { get; set; }
        public string tipoContrato { get; set; }
        public string dataSimulacao { get; set; }
        public string infoTabela { get; set; }
        public string acao { get; set; }
        public string modulo { get; set; }
        public bool recuperacaoJudicial { get; set; }

    }

}