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
using calculadora_api.Dao;

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
            List<ParceladoPreDao> parceladosPre = _context.ParceladoPreItems.Where(a => a.contractRef == contractRef).ToList();
            if (parceladosPre.Count > 0)
            {
                List<ParceladoPre> parcelados = new List<ParceladoPre>();
                foreach (ParceladoPreDao item in parceladosPre)
                {
                    parcelados.Add(item.parseFrom());
                } 
                return parcelados;
            }
            return NotFound();
        }


        //GET:      api/users/n
        [HttpGet("{id}")]
        public ActionResult<ParceladoPre> ParceladoPreItem(int id)
        {
            ParceladoPreDao parceladoPreItem = _context.ParceladoPreItems.Find(id);

            if (parceladoPreItem == null)
            {
                return NotFound();
            }

            return parceladoPreItem.parseFrom();
        }

        //POST:     api/users
        [HttpPost]
        public ActionResult PostParceladoPreItem(List<ParceladoPreDao> parceladoPreList)
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
        public ActionResult PutParceladoPreItem(List<ParceladoPreDao> parceladoPreList)
        {
            foreach (var parceladoPre in parceladoPreList)
            {
                _context.Entry(parceladoPreList).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }

        //DELETE:   api/users/n
        [HttpDelete("{id}")]
        public ActionResult<ParceladoPre> DeleteParceladoPreItem(int id)
        {
            ParceladoPreDao parceladoPreItem = _context.ParceladoPreItems.Find(id);

            if (parceladoPreItem == null)
            {
                return NotFound();
            }

            _context.ParceladoPreItems.Remove(parceladoPreItem);
            _context.SaveChanges();

            return parceladoPreItem.parseFrom();
        }


        [Route("incluir-parcelas")]
        [HttpPost]
        public ActionResult<JObject> incluirParcelas([FromBody] JObject dados)
        {
            string contractRef = dados.SelectToken("contractRef").ToString();
            InfoParaCalculo infoParaCalculo = InfoParaCalculo.parse(dados.SelectToken("infoParaCalculo"));
            List<Parcela> parcelas = Parcela.parse(dados.SelectToken("tableParcelas"));
            ParceladoPreService parceladoPreService = new ParceladoPreService(indiceController, infoParaCalculo);
            Tabela<ParceladoPre> table = new Tabela<ParceladoPre>();

            List<ParceladoPre> rgs = parceladoPreService.calcular(contractRef, parcelas);

            foreach (ParceladoPre item in rgs)
            {
                table.adicionarRegistro(item);
            }
            Totais totais = parceladoPreService.calcularTotais(table);
            Retorno<ParceladoPre> retorno = new Retorno<ParceladoPre>(contractRef, table, infoParaCalculo, totais);
            return tratarRetorno(retorno);
        }


        private ActionResult<JObject> tratarRetorno(Retorno<ParceladoPre> retorno) {
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(retorno));

            // retirar os objetos "parcela"
            obj.Descendants().OfType<JProperty>().Where(attr => attr.Name.Equals("parcela")).ToList().ForEach(attr => attr.Remove());
            return obj;
        }

    }
}