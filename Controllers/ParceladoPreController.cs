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
using Converter;
using System.Text.Json;

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
        public ActionResult<IEnumerable<Parcelado>> GetParceladoPreItems([FromQuery] string contractRef)
        {
            List<ParceladoPreDao> parceladosPre = _context.ParceladoPreItems.Where(a => a.contractRef == contractRef).ToList();
            if (parceladosPre.Count > 0)
            {
                List<Parcelado> parcelados = new List<Parcelado>();
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
        public ActionResult<Parcelado> ParceladoPreItem(int id)
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
        public ActionResult<Parcelado> DeleteParceladoPreItem(int id)
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

            TabelaParcelados table = parceladoPreService.calcular(contractRef, parcelas);
            TotaisParcelas totaisParcelas = parceladoPreService.calcularTotaisParcelas(table);
            TotaisRodape totais = parceladoPreService.calcularTotaisRodape(totaisParcelas);

            RetornoParcelado retorno = new RetornoParcelado(contractRef, table, infoParaCalculo, totais, totaisParcelas);
            return tratarRetorno(retorno);
        }


        private JObject tratarRetorno(Retorno<Parcelado> retorno) {
            // retirar os objetos "parcela"
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(retorno));
            obj.Descendants().OfType<JProperty>().Where(attr => attr.Name.Equals("parcela")).ToList().ForEach(attr => attr.Remove());
            return obj;
        }


        [Route("incluir-amortizacao")]
        [HttpPost]
        public ActionResult<JObject> incluirAmortizacao([FromBody] JObject dados)
        {

            RetornoParcelado retornoParcelado = RetornoParcelado.parse(
                dados.SelectToken("totais"),
                dados.SelectToken("infoParaCalculo"),
                dados.SelectToken("infoParaAmortizacao"),
                dados.SelectToken("tabela"),
                dados.SelectToken("rodape"),
                dados.SelectToken("contractRef")
            );
            
            ParceladoPreService service = new ParceladoPreService(indiceController, retornoParcelado.infoParaCalculo);
            service.calcularAmortizacao(
                retornoParcelado.infoParaAmortizacao,
                retornoParcelado.rodape,
                retornoParcelado.totais.totalParcelasVencidas
            );
            return Ok();
        }
    }
}