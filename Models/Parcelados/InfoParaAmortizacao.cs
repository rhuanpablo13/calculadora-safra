using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

public class InfoParaAmortizacao {

    public DateTime dataVencimento {get; set;}
    public float saldoDevedor {get; set;}
    public string tipo {get; set;}
    public bool wasUsed {get; set;}


    public InfoParaAmortizacao() {}



    public static List<InfoParaAmortizacao> parse(JToken infoParaAmortizacao) {
        List<InfoParaAmortizacao> info = infoParaAmortizacao.ToObject<List<InfoParaAmortizacao>>();
        return info;
    }


    public static InfoParaAmortizacao parse(string jsonString) {
        return JsonSerializer.Deserialize<InfoParaAmortizacao>(jsonString);
    }


    public override string ToString()
    {
        return " InfoParaAmortizacao: ["
        + "\n\t dataVencimento -> " + dataVencimento
        + "\n\t saldoDevedor -> " + saldoDevedor
        + "\n\t tipo -> " + tipo
        + "\n\t wasUsed -> " + wasUsed
        + "\n\t ]"
        ;
    }


}