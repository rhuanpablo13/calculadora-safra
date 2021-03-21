using System;
using System.ComponentModel.DataAnnotations;
using calculadora_api.Models;
using Converter;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Dao

{
    public class ChequeEmpresarialDao
    {
        [Key]
        public int id { get; set; }
        public string dataBase { get; set; }
        public string indiceDB { get; set; }
        public float indiceDataBase { get; set; }
        public string indiceBA { get; set; }
        public float indiceDataBaseAtual { get; set; }
        public string dataBaseAtual { get; set; }
        public float valorDevedor { get; set; }
        public string encargosMonetarios { get; set; }
        public float lancamentos { get; set; }
        public string tipoLancamento { get; set; }
        public float valorDevedorAtualizado { get; set; }
        public string contractRef { get; set; }
        public string ultimaAtualizacao { get; set; }
        public string infoParaCalculo { get; set; }
    
    
        public ChequeEmpresarial parseFrom() {
            ChequeEmpresarial c = new ChequeEmpresarial();
            c.id = this.id;
            c.dataBase = U.toDateTime(this.dataBase);
            c.indiceDB = this.indiceDB;
            c.indiceDataBase = this.indiceDataBase;
            c.indiceBA = this.indiceBA;
            c.indiceDataBaseAtual = this.indiceDataBaseAtual;
            c.dataBaseAtual = U.toDateTime(this.dataBaseAtual);
            c.valorDevedor = this.valorDevedor;
            c.encargosMonetarios = EncargosMonetarios.parse(this.encargosMonetarios);
            c.lancamentos = this.lancamentos;
            c.tipoLancamento = this.tipoLancamento;
            c.valorDevedorAtualizado = this.valorDevedorAtualizado;
            c.contractRef = this.contractRef;
            c.ultimaAtualizacao = U.toDateTime(this.ultimaAtualizacao);
            c.infoParaCalculo = InfoParaCalculo.parse(this.infoParaCalculo);
            return c;       
        }

    }






    // public class EncargosMonetarios
    // {
    //     [Key]
    //     public int Id { get; set; }
    //     public float correcaoPeloIndice { get; set; }
    //     public JurosAm jurosAm { get; set; }
    //     public float multa { get; set; }
    // }
    // public class JurosAm
    // {
    //     [Key]
    //     public int Id { get; set; }
    //     public float dias { get; set; }
    //     public float percentsJuros { get; set; }
    //     public float moneyValue { get; set; }
    // }
}