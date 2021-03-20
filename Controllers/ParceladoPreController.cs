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
        private readonly ApplicationContext _context;
        private readonly IndiceController indiceController;

        public ParceladoPreController(ApplicationContext context)
        {
            _context = context;
            if (indiceController == null)
                indiceController = new IndiceController(_context);
        }

        [HttpGet("pesquisar")]
        public ActionResult<IEnumerable<ParceladoPre>> GetParceladoPreItems([FromQuery] string contractRef)
        {
            List<ParceladoPre> parceladosPre = _context.ParceladoPreItems.Where(a => a.contractRef == contractRef).ToList();
            if (parceladosPre.Count > 0)
            {
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


        [Route("incluir-parcelas")]
        [HttpPost]
        public ActionResult<JObject> incluirParcelas([FromBody] JObject dados)
        {
            string contractRef = dados.SelectToken("contractRef").ToString();
            InfoParaCalculo infoParaCalculo = InfoParaCalculo.parse(dados.SelectToken("infoParaCalculo"));
            List<Parcela> parcelas = Parcela.parse(dados.SelectToken("tableParcelas"));
            ParceladoPreService parceladoPreService = new ParceladoPreService(indiceController);
            Tabela<ParceladoPre> table = new Tabela<ParceladoPre>();

            List<ParceladoPre> rgs = parceladoPreService.calcular(contractRef, infoParaCalculo, parcelas);

            foreach (ParceladoPre item in rgs)
            {
                table.adicionarRegistro(item);
            }
            Totais totais = parceladoPreService.calcularTotais(table);
            Retorno<ParceladoPre> retorno = new Retorno<ParceladoPre>(contractRef, table, totais);
            return JObject.Parse(JsonConvert.SerializeObject(retorno));

            // return JObject.Parse("{'success': false, 'msg':'Algo de errado aconteceu'}");
        }

    }
}