using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

public class InfoParaAmortizacao {

    public DateTime data_vencimento {get; set;}
    public float saldo_devedor {get; set;}
    public string tipo {get; set;}
    public float residual {get; set;} = 0;


    public InfoParaAmortizacao() {}



    public static List<InfoParaAmortizacao> parse(JToken infoParaAmortizacao) {
        if (infoParaAmortizacao == null) return new List<InfoParaAmortizacao>();
        List<InfoParaAmortizacao> info = infoParaAmortizacao.ToObject<List<InfoParaAmortizacao>>();
        return info;
    }


    public static List<InfoParaAmortizacao> parse(string jsonString) {
        return JsonSerializer.Deserialize<List<InfoParaAmortizacao>>(jsonString);
    }


    public override string ToString()
    {
        return " InfoParaAmortizacao: ["
        + "\n\t\t data_vencimento -> " + data_vencimento
        + "\n\t\t saldo_devedor -> " + saldo_devedor
        + "\n\t\t tipo -> " + tipo
        + "\n\t\t residual -> " + residual
        + "\n\t ]\n"
        ;
    }


}