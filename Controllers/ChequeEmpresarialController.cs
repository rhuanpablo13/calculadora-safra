using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using Converter;
using Newtonsoft.Json;
using System.Linq;
using Newtonsoft.Json.Linq;
using calculadora_api.Services;

using System.Security;
using System.Security.Principal;

namespace calculadora_api.Controllers
{
    [Route("api/cheque-empresarial")]
    [ApiController]
    public class ChequeEmpresarialController : ControllerBase
    {
        private readonly UserContext _context;

        private readonly IndiceController indiceController;

        private readonly JSON jc = new JSON();

        public ChequeEmpresarialController(UserContext context)
        {
            _context = context;
            if (indiceController == null)
                indiceController = new IndiceController(_context);
        }



        [HttpGet("pesquisar")]
        public ActionResult<IEnumerable<ChequeEmpresarial>> GetChequeEmpresarialItems([FromQuery] string contractRef)
        {
            List<ChequeEmpresarial> cheques = _context.ChequeEmpresarialItems.Where(a => a.contractRef == contractRef).ToList();
            if (cheques.Count > 0)
            {
                return cheques;
                //return calcular(cheques);
            }
            return NoContent();
        }


        //GET:/api/users/n
        [HttpGet("{id}")]
        public ActionResult<ChequeEmpresarial> ChequeEmpresarialItem(int id)
        {
            var chequeempresarialItem = _context.ChequeEmpresarialItems.Find(id);

            if (chequeempresarialItem == null)
            {
                return NotFound();
            }

            return chequeempresarialItem;
        }


        [HttpPost]
        public ActionResult PostChequeEmpresarialItem([FromBody] List<ChequeEmpresarial> chequeEmpresarialList)
        {
            Console.WriteLine("---------------------------------------------------");
            foreach (var chequeEmpresarial in chequeEmpresarialList)
            {
                _context.ChequeEmpresarialItems.Add(chequeEmpresarial);
                _context.SaveChanges();
            }
            return NoContent();
        }


        [HttpPut]
        public ActionResult PutChequeEmpresarialItem([FromBody] List<ChequeEmpresarial> chequeEmpresarialList)
        {
            foreach (var chequeEmpresarial in chequeEmpresarialList)
            {
                Console.WriteLine(chequeEmpresarial);

                _context.Entry(chequeEmpresarial).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult<ChequeEmpresarial> DeleteChequeEmpresarialItem(int id)
        {
            var chequeempresarialItem = _context.ChequeEmpresarialItems.Find(id);

            if (chequeempresarialItem == null)
            {
                return NotFound();
            }

            _context.ChequeEmpresarialItems.Remove(chequeempresarialItem);
            _context.SaveChanges();

            return chequeempresarialItem;
        }



        // Object dados -> json dos dados do formulário
        [Route("incluir-lancamento")]
        [HttpPost]
        public ActionResult<JObject> incluirLancamento([FromBody] JObject dados)
        {
            string contractRef = dados.SelectToken("contractRef").ToString();
            InfoLancamento infoLancamento = InfoLancamento.parse(dados.SelectToken("infoLancamento"));
            InfoParaCalculo infoParaCalculo = InfoParaCalculo.parse(dados.SelectToken("infoParaCalculo"));
            LancamentosService lancamentosService = new LancamentosService(indiceController);
            ChequeEmpresarial novoCheque;
            Tabela<ChequeEmpresarial> table = new Tabela<ChequeEmpresarial>();

            table.carregarRegistros(dados.SelectToken("table"));

            if (!table.temRegistros())
            {
                novoCheque = lancamentosService.calcular(contractRef, infoParaCalculo, infoLancamento);
            }
            else
            {
                novoCheque = lancamentosService.calcular(contractRef, infoParaCalculo, infoLancamento, table.getUltimoRegistro());
            }

            if (!novoCheque.isEmpty())
            {
                table.adicionarRegistro(novoCheque);
                Totais totais = lancamentosService.calcularTotais(table);
                Retorno retorno = new Retorno(contractRef, table, totais);
                return JObject.Parse(JsonConvert.SerializeObject(retorno));
            }

            return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        }

        // [Route("incluir-lancamento")]
        // [HttpPost]
        // public ActionResult<JObject> incluirLancamento([FromBody] JObject dados) {
        //     LancamentosService lancamentosService = new LancamentosService(indiceController);

        //     Tabela registros = lancamentosService.calcular(dados);
        //     if (registros != null) {
        //         Totais totais = lancamentosService.calcularTotais(registros);
        //         if (totais != null) {
        //             Retorno retorno = new Retorno("contrato infos", registros, totais);
        //             return JObject.Parse(
        //                 JsonConvert.SerializeObject(retorno)
        //             );
        //         }
        //     }
        //     return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        // }


    }
}