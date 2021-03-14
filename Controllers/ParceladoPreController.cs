using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Newtonsoft.Json.Linq;
using calculadora_api.Services;
using System;
using Newtonsoft.Json;

namespace calculadora_api.Controllers
{
    [Route("api/parcelado-pre")]
    [ApiController]
    public class ParceladoPreController : ControllerBase
    {
        private readonly UserContext _context;

        private readonly IUser _user;

        private IndiceController indiceController;

        public ParceladoPreController(UserContext context)
        {
            _context = context;
            if (indiceController == null) 
                indiceController = new IndiceController(_context);
        }

        [HttpGet("pesquisar")]
        public ActionResult<IEnumerable<ParceladoPre>> GetParceladoPreItems([FromQuery] string contractRef)
        {
            List<ParceladoPre> parceladosPre = _context.ParceladoPreItems.Where(a => a.contractRef == contractRef).ToList();
            if (parceladosPre.Count > 0) {
                return parceladosPre;
                //return calcular(cheques);
            }
            return NotFound(); // vai para o incluirLancamento
        }

        //GET:      api/users/n
        [HttpGet("{id}")]
        public ActionResult<ParceladoPre> ParceladoPreItem(int id)
        {
            var parceladoPreItem = _context.ParceladoPreItems.Find(id);

            if (parceladoPreItem == null)
            {
                return NotFound();
            }

            return parceladoPreItem;
        }

        //POST:     api/users
        [HttpPost]
        public ActionResult PostParceladoPreItem(List<ParceladoPre> parceladoPreList)
        {
            foreach (var parceladoPre in parceladoPreList)
            {
                _context.ParceladoPreItems.Add(parceladoPre);
                _context.SaveChanges();
            }
            return NoContent();
        }

        //PUT:      api/users/n
        [HttpPut]
        public ActionResult PutParceladoPreItem(List<ParceladoPre> parceladoPreList)
        {
            foreach (var parceladoPre in parceladoPreList)
            {
                _context.Entry(parceladoPre).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }

        //DELETE:   api/users/n
        [HttpDelete("{id}")]
        public ActionResult<ParceladoPre> DeleteParceladoPreItem(int id)
        {
            var parceladoPreItem = _context.ParceladoPreItems.Find(id);

            if (parceladoPreItem == null)
            {
                return NotFound();
            }

            _context.ParceladoPreItems.Remove(parceladoPreItem);
            _context.SaveChanges();

            return parceladoPreItem;
        }

        // Object dados -> json dos dados do formulário
        [Route("incluir-parcelas")]
        [HttpPost]
        public ActionResult<JObject> incluirParcelas([FromBody] JObject dados) {
            ParceladoPreService parceladoService = new ParceladoPreService(indiceController);
            
            Tabela registros = parceladoService.calcular(dados);
            Console.WriteLine("##############################################");
            Console.WriteLine(registros.ToString());
            
            if (registros != null) {
                Totais totais = parceladoService.calcularTotais(registros);
                if (totais != null) {
                    Retorno retorno = new Retorno("contrato infos", registros, totais);
                    return JObject.Parse(
                        JsonConvert.SerializeObject(retorno)
                    );
                }
            }
            return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        }
    }
}