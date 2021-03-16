using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class Tabela
    {
        private List<ChequeEmpresarial> registros { get; set; }

        public Tabela()
        {
            if (registros == null)
                registros = new List<ChequeEmpresarial>();
        }

        public void carregarRegistros(JToken table)
        {
            if (table != null)
            {
                registros.Clear();
                registros = table.ToObject<List<ChequeEmpresarial>>();
            }
        }

        public void adicionarRegistro(ChequeEmpresarial registro)
        {
            registros.Add(registro);
        }

        public bool temRegistros()
        {
            return registros.Count > 0;
        }

        public ChequeEmpresarial getUltimoRegistro()
        {
            return registros.Count > 0 ? registros[^1] : null;
        }

        public List<ChequeEmpresarial> getRegistros() => registros;


        internal JObject toJson()
        {
            return JObject.Parse(JsonConvert.SerializeObject(registros));
        }


        public override string ToString()
        {
            string str = "";
            if (registros.Count > 0)
            {
                foreach (ChequeEmpresarial item in registros)
                {
                    str += item.ToString() + "\n";
                }
            }
            return str;
        }
    }

}