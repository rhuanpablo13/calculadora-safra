using System;
using System.ComponentModel.DataAnnotations;
using Converter;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class ChequeEmpresarial
    {

        [Key]
        public int id { get; set; }
        public string dataBase { get; set; }
        public string indiceDB { get; set; }
        public double indiceDataBase { get; set; }
        public string indiceBA { get; set; }
        public double indiceDataBaseAtual { get; set; }
        public string dataBaseAtual { get; set; }
        public double valorDevedor { get; set; }
        public string encargosMonetarios { get; set; }
        public double lancamentos { get; set; }
        public string tipoLancamento { get; set; }
        public double valorDevedorAtualizado { get; set; }
        public string contractRef { get; set; }
        public string ultimaAtualizacao { get; set; }
        public string infoParaCalculo { get; set; }


         public void copyFromChequeEmpresarialBack(ChequeEmpresarialBack cb) {            
            // id = cb.id > -1 ? cb.id : -1;
            // dataBase = cb.dataBase.ToString("yyyy-MM-dd");
            // indiceDB = cb.indiceDB;
            // indiceDataBase = cb.indiceDataBase;
            // indiceBA = cb.indiceBA;
            // indiceDataBaseAtual = cb.indiceDataBaseAtual;
            // dataBaseAtual = cb.dataBaseAtual.ToString("yyyy-MM-dd");
            // valorDevedor = cb.valorDevedor;
            // encargosMonetarios = JSON.toStringJson<EncargosMonetarios>(cb.encargosMonetarios).ToString();
            // lancamentos = cb.lancamentos;
            // tipoLancamento = cb.tipoLancamento;
            // valorDevedorAtualizado = cb.valorDevedorAtualizado;
            // contractRef = cb.contractRef;
            // ultimaAtualizacao = cb.ultimaAtualizacao.ToString("yyyy-MM-dd");
            // infoParaCalculo = JSON.toStringJson<InfoParaCalculo>(cb.infoParaCalculo).ToString();
        }

        public override string ToString() {
            return "Cheque Empresarial: ["
            + "\n\t id -> " + id
            + "\n\t dataBase -> " + dataBase
            + "\n\t indiceDB -> " + indiceDB
            + "\n\t indiceDataBase -> " + indiceDataBase
            + "\n\t indiceBA -> " + indiceBA
            + "\n\t indiceDataBaseAtual -> " + indiceDataBaseAtual
            + "\n\t dataBaseAtual -> " + dataBaseAtual
            + "\n\t valorDevedor -> " + valorDevedor
            + "\n\t encargosMonetarios -> " + encargosMonetarios
            + "\n\t lancamentos -> " + lancamentos
            + "\n\t tipoLancamento -> " + tipoLancamento
            + "\n\t valorDevedorAtualizado -> " + valorDevedorAtualizado
            + "\n\t contractRef -> " + contractRef
            + "\n\t ultimaAtualizacao -> " + ultimaAtualizacao
            + "\n\t infoParaCalculo -> " + infoParaCalculo 
            + "]\n\n"
            ;
        }
    }

    public class JurosAm    {
        public int dias { get; set; } 
        public double percentsJuros { get; set; } 
        public double moneyValue { get; set; } 

        public override string ToString() {
            return " Juros AM: ["
            + "\n\t\t\t dias -> " + dias
            + "\n\t\t\t percentsJuros -> " + percentsJuros
            + "\n\t\t\t moneyValue -> " + moneyValue + "\n\t\t ]"
            ;
        }
    }

    

    

    





    public class ChequeEmpresarialBack
    {

        [Key]
        public int id { get; set; }
        public DateTime dataBase { get; set; }
        public string indiceDB { get; set; }
        public double indiceDataBase { get; set; }
        public string indiceBA { get; set; }
        public double indiceDataBaseAtual { get; set; }
        public DateTime dataBaseAtual { get; set; }
        public double valorDevedor { get; set; }
        public EncargosMonetarios encargosMonetarios { get; set; }
        public double lancamentos { get; set; }
        public string tipoLancamento { get; set; }
        public double valorDevedorAtualizado { get; set; }
        public string contractRef { get; set; }
        public DateTime ultimaAtualizacao { get; set; }
        public InfoParaCalculo infoParaCalculo { get; set; }


        public ChequeEmpresarialBack() {
            encargosMonetarios = new EncargosMonetarios();
            infoParaCalculo = new InfoParaCalculo();
        }


        public ChequeEmpresarialBack(DadosLancamento dadosLancamento) {
            encargosMonetarios = new EncargosMonetarios();
            infoParaCalculo = new InfoParaCalculo();
            copyFromDadosLancamento(dadosLancamento);
        }


        public void copyFromChequeEmpresarial(ChequeEmpresarial chequeEmpresarial) {

            this.id = chequeEmpresarial.id;
            this.dataBase = (
                chequeEmpresarial.dataBase != null
                ?  DateTime.ParseExact(chequeEmpresarial.dataBase, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                : zeroDate()
            );

            this.indiceDB = chequeEmpresarial.indiceDB != null ? chequeEmpresarial.indiceDB : "";
            this.indiceDataBase = chequeEmpresarial.indiceDataBase;
            this.indiceBA = chequeEmpresarial.indiceBA != null ? chequeEmpresarial.indiceBA : "";
            this.indiceDataBaseAtual = chequeEmpresarial.indiceDataBaseAtual;

            this.dataBaseAtual = (
                chequeEmpresarial.dataBaseAtual != null
                ? DateTime.ParseExact(chequeEmpresarial.dataBaseAtual, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
                : zeroDate() 
            );

            this.valorDevedor = chequeEmpresarial.valorDevedor;
            
            EncargosMonetarios encargosMonetarios = new EncargosMonetarios();
            if (chequeEmpresarial.encargosMonetarios != null)
                // encargosMonetarios = JSON.toObject<EncargosMonetarios>(chequeEmpresarial.encargosMonetarios);
            this.encargosMonetarios = encargosMonetarios;

            this.lancamentos = chequeEmpresarial.lancamentos;
            this.tipoLancamento = chequeEmpresarial.tipoLancamento != null ? chequeEmpresarial.tipoLancamento : "";
            this.valorDevedorAtualizado = chequeEmpresarial.valorDevedorAtualizado;
            this.contractRef = chequeEmpresarial.contractRef != null ? chequeEmpresarial.contractRef : "";
            this.ultimaAtualizacao = chequeEmpresarial.ultimaAtualizacao != null 
            ? DateTime.ParseExact(chequeEmpresarial.ultimaAtualizacao, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            : zeroDate();

            InfoParaCalculo infoParaCalculo = new InfoParaCalculo();
            if (chequeEmpresarial.infoParaCalculo != null)
                infoParaCalculo = JSON.toObject<InfoParaCalculo>(chequeEmpresarial.infoParaCalculo);
            this.infoParaCalculo = infoParaCalculo;
        }
       
        public void copyFromDadosLancamento(DadosLancamento dadosLancamento) {            

            contractRef = dadosLancamento.contractRef;
            
            // ultimaAtualizacao = dataVencimento
            ultimaAtualizacao = DateTime.ParseExact(dadosLancamento.dataVencimento, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);            
            infoParaCalculo.formDataCalculo = DateTime.ParseExact(dadosLancamento.formDataCalculo, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            infoParaCalculo.formMulta = dadosLancamento.formMulta;

            infoParaCalculo.formJuros = dadosLancamento.formJuros;
            infoParaCalculo.formHonorarios = dadosLancamento.formHonorarios;
            infoParaCalculo.formMultaSobContrato = dadosLancamento.formMultaSobContrato;

            infoParaCalculo.formIndice = dadosLancamento.formIndice;
            infoParaCalculo.formIndiceEncargos = dadosLancamento.formIndiceEncargos;
            infoParaCalculo.isDate = false;

            dataBase = ultimaAtualizacao;
            indiceDB = dadosLancamento.formIndice;
            indiceBA = dadosLancamento.formIndice;

            dataBaseAtual = DateTime.ParseExact(dadosLancamento.dataBaseAtual, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            valorDevedor = dadosLancamento.saldoDevedor;
            lancamentos = dadosLancamento.valorLancamento;
            tipoLancamento = dadosLancamento.tipoLancamento;

        }

        private DateTime zeroDate() { 
            return DateTime.ParseExact("0001-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        }


        public override string ToString() {
            return "Cheque Empresarial: ["
            + "\n\t id -> " + id
            + "\n\t dataBase -> " + dataBase
            + "\n\t indiceDB -> " + indiceDB
            + "\n\t indiceDataBase -> " + indiceDataBase
            + "\n\t indiceBA -> " + indiceBA
            + "\n\t indiceDataBaseAtual -> " + indiceDataBaseAtual
            + "\n\t dataBaseAtual -> " + dataBaseAtual
            + "\n\t valorDevedor -> " + valorDevedor
            + "\n\t encargosMonetarios -> " + encargosMonetarios
            + "\n\t lancamentos -> " + lancamentos
            + "\n\t tipoLancamento -> " + tipoLancamento
            + "\n\t valorDevedorAtualizado -> " + valorDevedorAtualizado
            + "\n\t contractRef -> " + contractRef
            + "\n\t ultimaAtualizacao -> " + ultimaAtualizacao
            + "\n\t infoParaCalculo -> " + infoParaCalculo 
            + "\n]\n\n"
            ;
        }

    }


    
}
