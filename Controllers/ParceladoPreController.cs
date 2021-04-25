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
        public ActionResult<IEnumerable<RegistroParcela>> GetParceladoPreItems([FromQuery] string contractRef)
        {
            List<ParceladoPreDao> parceladosPre = _context.ParceladoPreItems.Where(a => a.contractRef == contractRef).ToList();
            if (parceladosPre.Count > 0)
            {
                List<RegistroParcela> parcelados = new List<RegistroParcela>();
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
        public ActionResult<RegistroParcela> ParceladoPreItem(int id)
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
        public ActionResult<RegistroParcela> DeleteParceladoPreItem(int id)
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

            List<ParcelaInicial> parcelas = ParcelaInicial.parse(dados.SelectToken("tableParcelas"));
            ParceladoPreService parceladoPreService = new ParceladoPreService(indiceController, infoParaCalculo);

            Tabela<RegistroParcela> registros = new Tabela<RegistroParcela>();
            foreach (var parcela in parcelas)
            {
                RegistroParcela parcelado = new RegistroParcela();
                parcelado.carregarDadosEntrada(contractRef, infoParaCalculo, parcela);
                parcelado = parceladoPreService.calcular(parcelado);
                registros.adicionarRegistro(parcelado);
            }

            TotaisParcelas totaisParcelas = parceladoPreService.calcularTotaisParcelas(registros);
            TotaisRodape totais = parceladoPreService.calcularTotaisRodape(totaisParcelas);

            Parcelado retorno = new Parcelado(contractRef, registros.getRegistros(), infoParaCalculo, totais, totaisParcelas);
            return tratarRetorno(retorno);
        }


        private JObject tratarRetorno(Retorno<RegistroParcela> retorno) {
            // retirar os objetos "parcela"
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(retorno));
            obj.Descendants().OfType<JProperty>().Where(attr => attr.Name.Equals("parcela")).ToList().ForEach(attr => attr.Remove());
            return obj;
        }


        [Route("incluir-amortizacao")]
        [HttpPost]
        public ActionResult<JObject> incluirAmortizacao([FromBody] JObject dados)
        {
            Parcelado retornoParcelado = Parcelado.parse(
                dados.SelectToken("totais"),
                dados.SelectToken("infoParaCalculo"),
                dados.SelectToken("infoParaAmortizacao"),
                dados.SelectToken("tabela"),
                dados.SelectToken("rodape"),
                dados.SelectToken("contractRef")
            );


            ParceladoPreService service = new ParceladoPreService(indiceController, retornoParcelado.infoParaCalculo);
            Parcelado retorno = service.calcularAmortizacao(
                retornoParcelado.infoParaAmortizacao,
                retornoParcelado.rodape,
                retornoParcelado.totais.totalParcelasVencidas,
                retornoParcelado.tabela,
                retornoParcelado.contractRef
            );

            // Console.WriteLine(retorno);

            if (retorno == null) return JObject.Parse(JsonConvert.SerializeObject(retornoParcelado));

            retorno.contractRef = retornoParcelado.contractRef;
            retorno.infoParaCalculo = retornoParcelado.infoParaCalculo;
            return JObject.Parse(JsonConvert.SerializeObject(retorno));
        }


        [Route("incluir-amortizacao-teste")]
        [HttpGet]
        public ActionResult<Parcelado> incluirAmortizacao()
        {

            var infoParaCalculo = new InfoParaCalculo();
            infoParaCalculo.formDataCalculo = U.toDateTime("2019-04-15");
            infoParaCalculo.formMulta = 1.0f;
            infoParaCalculo.formJuros = 2.0f;
            infoParaCalculo.formHonorarios = 3.0f;
            infoParaCalculo.formMultaSobContrato = 4.0f;
            infoParaCalculo.formIndice = "INPC/IBGE";
            infoParaCalculo.formIndiceEncargos = 0.0f;
            infoParaCalculo.formDesagio = 5;
            infoParaCalculo.isDate = false;


            List<InfoParaAmortizacao> ams = new List<InfoParaAmortizacao>();
            InfoParaAmortizacao ia = new InfoParaAmortizacao();
            ia.data_vencimento = U.toDateTime("2019-01-10");
            ia.saldo_devedor = 10;
            ia.tipo = "Data Diferenciada";
            ams.Add(ia);

            var totaisRodape = new TotaisRodape();
            totaisRodape.subtotal = 216.97397f;
            totaisRodape.honorario = 6.5092187f;
            totaisRodape.multa = 8.939327f;
            totaisRodape.total = 232.42252f;
            totaisRodape.amortizacao = 0.0f;

            var totalParcelasVencidas = new TotalParcelasVencidas();
            totalParcelasVencidas.valorNoVencimento = 200.0f;
            totalParcelasVencidas.correcaoPeloIndice = 2.8571472f;
            totalParcelasVencidas.money = 11.968572f;
            totalParcelasVencidas.somaMulta = 2.1482573f;
            totalParcelasVencidas.subTotal = 216.97397f;
            totalParcelasVencidas.amortizacao = 0.0f;
            totalParcelasVencidas.totalDevedor = 216.9739f;

            var totaisParcelas = new TotaisParcelas();
            totaisParcelas.totalParcelasVencidas = totalParcelasVencidas;

            ParceladoPreService service = new ParceladoPreService(indiceController, infoParaCalculo);
            service.calcularAmortizacao(
                ams,
                totaisRodape,
                totalParcelasVencidas,
                gerarTabela().getRegistros(),
                "contrato infos"
            );

            Parcelado retorno = new Parcelado();
            retorno.tabela = service.tabela.getRegistros();
            retorno.totais = totaisParcelas;
            retorno.contractRef = "SBA.132395/201414005952556 - MUTUO PREfalse";
            retorno.infoParaAmortizacao = ams;
            retorno.infoParaCalculo = infoParaCalculo;
            //retorno.rodape = service.rodape;
            
            // retorno.totais = totaisParcelas;

            

            return Ok(JsonConvert.SerializeObject(retorno));
        }


        private Tabela<RegistroParcela> gerarTabela() {
            RegistroParcela p1 = new RegistroParcela();
            p1.Id = 0;
            p1.nparcelas = "1";
            p1.parcelaInicial = 1.0f;
            p1.dataVencimento = U.toDateTime("2019-01-01");
            p1.valorNoVencimento = 100.0f;
            p1.status = "aberto";
            p1.indiceDV = "INPC/IBGE";
            p1.indiceDataVencimento = 70.0f;
            p1.indiceDCA = "INPC/IBGE";
            p1.indiceDataCalcAmor = 71.0f;
            p1.dataCalcAmor = U.toDateTime("2019-04-15");
            p1.subtotal = 109.54556f;
            p1.valorPMTVincenda = 0.0f;
            p1.amortizacao = 0.0f;
            p1.totalDevedor = 109.54556f;
            p1.contractRef = "SBA.132395/201414005952556 - MUTUO PREfalse";
            p1.ultimaAtualizacao = new DateTime();
            p1.tipoParcela = null;
            p1.vincenda = false;

            EncargosMonetarios em1 = new EncargosMonetarios();
            JurosAm am1 = new JurosAm();
            am1.dias = 104;
            am1.percentsJuros = 6.933334f;
            am1.moneyValue = 7.032381f;
            em1.jurosAm = am1;
            em1.correcaoPeloIndice = 1.4285736f;
            em1.multa = 1.0846095f;
            p1.encargosMonetarios = em1;


            RegistroParcela p2 = new RegistroParcela();
            p2.Id = 0;
            p2.nparcelas = "2";
            p2.parcelaInicial = 1.0f;
            p2.dataVencimento = U.toDateTime("2019-02-01");
            p2.valorNoVencimento = 100.0f;
            p2.status = "aberto";
            p2.indiceDV = "INPC/IBGE";
            p2.indiceDataVencimento = 70.0f;
            p2.indiceDCA = "INPC/IBGE";
            p2.indiceDataCalcAmor = 71.0f;
            p2.dataCalcAmor = U.toDateTime("2019-04-15");
            p2.subtotal = 107.428406f;
            p2.valorPMTVincenda = 0.0f;
            p2.amortizacao = 0.0f;
            p2.totalDevedor = 107.428406f;
            p2.contractRef = "SBA.132395/201414005952556 - MUTUO PREfalse";
            p2.ultimaAtualizacao = new DateTime();
            p2.tipoParcela = null;
            p2.vincenda = false;

            EncargosMonetarios em2 = new EncargosMonetarios();
            JurosAm am2 = new JurosAm();
            am2.dias = 73;
            am2.percentsJuros = 4.866667f;
            am2.moneyValue = 4.9361906f;
            em2.jurosAm = am2;
            em2.correcaoPeloIndice =  1.4285736f;
            em2.multa = 1.0636476f;
            p2.encargosMonetarios = em2;
            
            Tabela<RegistroParcela> tabela = new Tabela<RegistroParcela>();
            tabela.adicionarRegistro(p1);
            tabela.adicionarRegistro(p2);
            return tabela;
        }
        
    }
}