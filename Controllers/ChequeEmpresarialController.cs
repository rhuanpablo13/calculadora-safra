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
using calculadora_api.Dao;

namespace calculadora_api.Controllers
{
    [Route("api/cheque-empresarial")]
    [ApiController]
    public class ChequeEmpresarialController : ControllerBase
    {
        private readonly ApplicationContext _context;

        private readonly IndiceController indiceController;

        private readonly U jc = new U();

        public ChequeEmpresarialController(ApplicationContext context)
        {
            _context = context;
            if (indiceController == null)
                indiceController = new IndiceController(_context);
        }



        [HttpGet("pesquisar")]
        public ActionResult<IEnumerable<ChequeEmpresarial>> GetChequeEmpresarialItems([FromQuery] string contractRef)
        {
            List<ChequeEmpresarialDao> chequesDao = _context.ChequeEmpresarialItems.Where(a => a.contractRef == contractRef).ToList();
            if (chequesDao.Count > 0)
            {
                List<ChequeEmpresarial> cheques = new List<ChequeEmpresarial>();
                foreach (ChequeEmpresarialDao item in chequesDao)
                {
                    cheques.Add(item.parseFrom());
                } 
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

            return chequeempresarialItem.parseFrom();
        }


        [HttpPost]
        public ActionResult PostChequeEmpresarialItem([FromBody] List<ChequeEmpresarial> chequeEmpresarialList)
        {
            foreach (var chequeEmpresarial in chequeEmpresarialList)
            {
                _context.ChequeEmpresarialItems.Add(chequeEmpresarial.parse());
                _context.SaveChanges();
            }
            return NoContent();
        }


        [HttpPut]
        public ActionResult PutChequeEmpresarialItem([FromBody] List<ChequeEmpresarial> chequeEmpresarialList)
        {
            foreach (ChequeEmpresarial chequeEmpresarial in chequeEmpresarialList)
            {
                ChequeEmpresarialDao dao = chequeEmpresarial.parse();
                _context.Entry(dao).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult<ChequeEmpresarial> DeleteChequeEmpresarialItem(int id)
        {
            ChequeEmpresarialDao chequeempresarialItem = _context.ChequeEmpresarialItems.Find(id);

            if (chequeempresarialItem == null)
            {
                return NotFound();
            }

            _context.ChequeEmpresarialItems.Remove(chequeempresarialItem);
            _context.SaveChanges();

            return chequeempresarialItem.parseFrom();
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
                TotaisRodape totais = lancamentosService.calcularTotais(table);
                Retorno<ChequeEmpresarial> retorno = new Retorno<ChequeEmpresarial>(contractRef, table.getRegistros(), totais);
                return JObject.Parse(JsonConvert.SerializeObject(retorno));
            }

            return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        }


    }
}