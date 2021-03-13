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

    public class EncargosMonetarios    {
        public double correcaoPeloIndice { get; set; } 
        public JurosAm jurosAm { get; set; } 
        public double multa { get; set; } 

        public EncargosMonetarios() {
            jurosAm = new JurosAm();
        }

        public override string ToString() {
            return "Encargos Monetarios: ["
            + "\n\t\t correcaoPeloIndice -> " + correcaoPeloIndice
            + "\n\t\t jurosAm -> " + jurosAm
            + "\n\t\t multa -> " + multa
            + "\n\t ]"
            ;
        }
    }

    public class InfoContrato    {
        public string pasta { get; set; } 
        public string contrato { get; set; } 
        public string tipo_contrato { get; set; } 
        public string contractRef { get; set; } 
        public bool recuperacaoJudicial { get; set; } 
        public string cliente { get; set; } 
        public string cnpj { get; set; } 

        public override string ToString() {
            return "Info Contrato: ["
            + "\n\t\t\t pasta -> " + pasta
            + "\n\t\t\t contrato -> " + contrato
            + "\n\t\t\t tipo_contrato -> " + tipo_contrato
            + "\n\t\t\t contractRef -> " + contractRef
            + "\n\t\t\t recuperacaoJudicial -> " + recuperacaoJudicial
            + "\n\t\t\t cliente -> " + cliente
            + "\n\t\t\t cnpj -> " + cnpj
            + "\n\t\t]\n"
            ;
        }
        
    }

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
                encargosMonetarios = JSON.toObject<EncargosMonetarios>(chequeEmpresarial.encargosMonetarios);
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


    public class DadosLancamento {
        
        private dynamic dynamic { get; set; }

        public string contractRef { get; set; }
        public string formDataCalculo { get; set; }
        public double formMulta { get; set; }
        public double formJuros { get; set; }
        public double formHonorarios { get; set; }
        public double formMultaSobContrato { get; set; }
        public string formIndice { get; set; }
        public double formIndiceEncargos { get; set; }
        public string dataVencimento { get; set; }
        public double saldoDevedor { get; set; }
        public string tipoLancamento { get; set; }
        public string dataBaseAtual { get; set; }
        public double valorLancamento { get; set; }



        public DadosLancamento(JObject dados) {
            Object tmp = dados.ToObject<Object>();
            dynamic = JsonConvert.DeserializeObject(dados.ToString());
        }


        public DadosLancamento(dynamic dynamic) {
            this.dynamic = dynamic;
        }


        public void parse() {
            contractRef = dynamic["contractRef"];
            formDataCalculo = dynamic["infoParaCalculo"]["formDataCalculo"];
            formMulta = dynamic["infoParaCalculo"]["formMulta"];
            formJuros = dynamic["infoParaCalculo"]["formJuros"];
            formHonorarios = dynamic["infoParaCalculo"]["formHonorarios"];
            formMultaSobContrato = dynamic["infoParaCalculo"]["formMultaSobContrato"];
            formIndice = dynamic["infoParaCalculo"]["formIndice"];
            formIndiceEncargos = dynamic["infoParaCalculo"]["formIndiceEncargos"];
            dataVencimento = dynamic["infoLancamento"]["dataVencimento"];
            saldoDevedor = dynamic["infoLancamento"]["saldoDevedor"];
            tipoLancamento = dynamic["infoLancamento"]["tipoLancamento"];
            dataBaseAtual = dynamic["infoLancamento"]["dataBaseAtual"];
            valorLancamento = dynamic["infoLancamento"]["valorLancamento"];
        }

        public override string ToString() {
            return "RegistroParaCalculo: ["
            + "\n\t formDataCalculo -> " + formDataCalculo
            + "\n\t formMulta -> " + formMulta
            + "\n\t formJuros -> " + formJuros
            + "\n\t formHonorarios -> " + formHonorarios
            + "\n\t formMultaSobContrato -> " + formMultaSobContrato
            + "\n\t formIndice -> " + formIndice
            + "\n\t formIndiceEncargos -> " + formIndiceEncargos
            + "\n\t dataVencimento -> " + dataVencimento
            + "\n\t saldoDevedor -> " + saldoDevedor
            + "\n\t tipoLancamento -> " + tipoLancamento
            + "\n\t dataBaseAtual -> " + dataBaseAtual
            + "\n\t valorLancamento -> " + valorLancamento
            + "\n]\n\n";
        }    
    }
}
