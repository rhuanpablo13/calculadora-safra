using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class Tabela
    {
        public Tabela() {
            registros = new List<ChequeEmpresarialBack>();
        }



        public void carregarRegistros(JObject dados) {
            var temp = dados.SelectToken("table");
            if (temp != null) {
                this.registros.Clear();
                this.registros = temp.ToObject<List<ChequeEmpresarialBack>>();
            }
        }


        public void carregarRegistros(ChequeEmpresarialBack[] registros) {
            this.registros.Clear();
            this.registros.AddRange(registros);
        }

        public void carregarRegistros(List<ChequeEmpresarialBack> registros) {
            this.registros.Clear();
            this.registros.AddRange(registros);
        }

        public void carregarRegistros(JArray registros, bool deletarRegistrosAnteriores) {
            if (deletarRegistrosAnteriores)
                registros.Clear();
            this.registros.AddRange(registros.Values<ChequeEmpresarialBack>());
        }

        public void carregarRegistros(JArray registros) {
            this.registros.AddRange(registros.Values<ChequeEmpresarialBack>());
        }

        public void adicionarRegistro(ChequeEmpresarialBack registro) {
            this.registros.Add(registro);
        }

        public bool temRegistros() {
            return registros.Count > 0;
        }

        public ChequeEmpresarialBack getUltimoRegistro() {
            return registros.Count > 0 ? registros[registros.Count-1] : null;
        }

        private List<ChequeEmpresarialBack> registros { get;set; }


        public override string ToString() {
            string str = "";
            if (registros.Count > 0) {
                foreach (ChequeEmpresarialBack item in registros) {
                    str += item.ToString() + "\n";
                }
            }
            return str;
        }
    }

}   