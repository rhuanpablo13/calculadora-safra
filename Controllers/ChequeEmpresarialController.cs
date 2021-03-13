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

        private IndiceController indiceController;

        private JSON jc = new JSON();

        private readonly IUser _user;

        public ChequeEmpresarialController(UserContext context, IUser user)
        {
            _user = user;
            _context = context;
            if (indiceController == null) 
                indiceController = new IndiceController(_context);            
        }


        [HttpGet("contractRef")]
        public ActionResult<IEnumerable<ChequeEmpresarial>> GetChequeEmpresarialItems([FromQuery] string contractRef)
        {
            var https = HttpContext.Request.IsHttps;
            var caminho = HttpContext.Request.Path;
            var status = HttpContext.Response.StatusCode;
            var conexao = HttpContext.Connection.ToString();
            Console.WriteLine(https + "\r\n" + caminho + "\r\n" + status + "\r\n" + conexao);

            Console.WriteLine("--------------------------");
            Console.WriteLine(HttpContext.User.Identity.Name);
            Console.WriteLine(HttpContext.User.Identity.AuthenticationType);
            Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);
            Console.WriteLine(HttpContext.User.Claims.Count());
            Console.WriteLine(HttpContext.User.Identities.Count());

            


            // Console.WriteLine(_user.Name);
            // Console.WriteLine(_user.Nome);

            // List<ChequeEmpresarial> cheques = _context.ChequeEmpresarialItems.Where(a => a.contractRef == contractRef).ToList();
            // if (cheques.Count > 0) {
            //     return cheques;
            //     //return calcular(cheques);
            // }
            // return NotFound(); // vai para o incluirLancamento
            return Ok();
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
        public ActionResult<JObject> incluirLancamento([FromBody] JObject dados) {
            LancamentosService lancamentosService = new LancamentosService(indiceController);
            
            Tabela registros = lancamentosService.calcular(dados);
            Console.WriteLine("##############################################");
            Console.WriteLine(registros.ToString());
            
            if (registros != null) {
                Totais totais = lancamentosService.calcularTotais(registros);
                if (totais != null) {
                    Retorno retorno = new Retorno("contrato infos", registros, totais);
                    return JObject.Parse(
                        JsonConvert.SerializeObject(retorno)
                    );
                }
            }
            return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        }



        // // Object dados -> json dos dados do formulário
        // [Route("incluir-lancamento")]
        // [HttpPost]
        // public String incluirLancamento([FromBody] Object dados) {            
            
        //     dynamic dynamic = JsonConvert.DeserializeObject(dados.ToString());
        //     DadosLancamento dadosLancamento = new DadosLancamento(dynamic);

        //     ChequeEmpresarialBack cheque = new ChequeEmpresarialBack(dadosLancamento);
        //     LancamentosService lancamentosService = new LancamentosService(indiceController);

        //     List<ChequeEmpresarialBack> registros = new List<ChequeEmpresarialBack>();
        //     cheque = lancamentosService.calcular(cheque, dynamic["table"]);
        //     registros.Add(cheque);            
        //     return lancamentosService.calcularTotais(registros, dadosLancamento);
        // } 

    }
}