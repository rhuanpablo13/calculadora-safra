using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace calculadora_api.Models
{
    public class Tabela<T>
    {
        private List<T> registros { get; set; }

        public Tabela()
        {
            if (registros == null)
                registros = new List<T>();
        }

        // TODO: validar o carregar registros
        public void carregarRegistros(JToken table)
        {
            if (table != null)
            {
                registros.Clear();
                registros = table.ToObject<List<T>>();
            }
        }

        public void adicionarRegistro(T registro)
        {
            registros.Add(registro);
        }

        public bool temRegistros()
        {
            return registros.Count > 0;
        }

        public T getUltimoRegistro()
        {
            return registros.Count > 0 ? registros[^1] : default(T);
        }

        public List<T> getRegistros() => registros;

        
        public T get(int index) {
            if (registros == null || registros.Count == 0 || registros.Count < index) {
                return default(T);
            }      
            return registros[index];
        }

        public void remove(int index) {
            if (registros == null || registros.Count == 0 || registros.Count < index) {
                return;
            }            
            registros.RemoveAt(index);
        }

        public void update(int index, T newObj) {
            if (registros == null || registros.Count == 0 || registros.Count < index) {
                return;
            }            
            registros[index] = newObj;
        }

        public bool exists(int index) {
            if (registros == null || registros.Count == 0) {
                return false;
            }
            return registros[index] != null;
        }

        public override string ToString()
        {
            string str = "\t";
            if (registros.Count > 0)
            {
                foreach (T item in registros)
                {
                    str += item.ToString() + "\t\t\n";
                }
            }
            return str;
        }
    }

}