using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using calculadora_api.Extensions;

namespace calculadora_api.Controllers
{
    [Route("api/log")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public LogController(ApplicationContext context)
        {
            _context = context;
        }

        //GET:      api/logs/log
        [HttpGet]
        // [Authorize(Roles = "admin")]
        public ActionResult<IEnumerable<Log>> GetLogItems([FromQuery] string pasta, [FromQuery] string contrato, [FromQuery] string tipoContrato, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0, [FromQuery] int draw = 1, [FromQuery] bool getAll = false, [FromQuery] bool recuperacaoJudicial = false)
        {
            IQueryable<Log> _logListByParameter = _context.LogItems
                                    .Where(item => item.pasta == pasta && item.contrato == contrato && item.tipoContrato == tipoContrato && item.recuperacaoJudicial == recuperacaoJudicial)
                                    .OrderByDescending(x => x.data);

            int recordsTotal = _logListByParameter.Count();

            if (getAll)
            {
                return _logListByParameter.ToList();
            }
            else
            {
                var data = _logListByParameter
                       .Skip(pageSize * pageNumber)
                       .Take(pageSize)
                       .ToList();

                return new ObjectResult(new { draw, recordsTotal, data });
            }

        }

        //GET:      api/users/n
        [HttpGet("{id}")]
        public ActionResult<Log> LogItem(int id)
        {
            var logItem = _context.LogItems.Find(id);

            if (logItem == null)
            {
                return NotFound();
            }

            return logItem;
        }

        // POST:     api/users
        [HttpPost]
        public ActionResult PostLogItem(List<Log> logList)
        {
            foreach (var log in logList)
            {
                _context.LogItems.Add(log);
                _context.SaveChanges();
            }
            return NoContent();
        }

        [HttpPost("pagination")]
        public ActionResult<IEnumerable<Log>> getLogPage(
        [FromQuery] string pasta,
        [FromQuery] string contrato,
        [FromQuery] string tipoContrato,
        [FromBody] DtParameters dtParameters,
        [FromQuery] int pageSize = 10,
        [FromQuery] int pageNumber = 0, [FromQuery] int draw = 1,
        [FromQuery] bool getAll = false,
        [FromQuery] bool recuperacaoJudicial = false)
        {
            if (dtParameters.Order != null)
            {
                IQueryable<Log> _logListByParameter = _context.LogItems;
                var searchBy = dtParameters.Search?.Value;
                var orderCriteria = string.Empty;
                var orderAscendingDirection = true;

                if (dtParameters.Order != null)
                {
                    // in this example we just default sort on the 1st column
                    orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                    orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
                }
                else
                {
                    // if we have an empty search then just order the results by Id ascending
                    orderCriteria = "Id";
                    orderAscendingDirection = true;
                }


                if (!string.IsNullOrEmpty(searchBy))
                {
                    _logListByParameter = _logListByParameter.Where(r =>
                                               r.Id >= 0 && r.Id.Equals(searchBy.ToUpper()) ||
                                               r.data != null && r.data.Equals(searchBy.ToUpper()) ||
                                               r.usuario != null && r.usuario.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.pasta != null && r.pasta.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.contrato != null && r.contrato.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.tipoContrato != null && r.tipoContrato.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.dataSimulacao != null && r.dataSimulacao.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.infoTabela != null && r.infoTabela.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.acao != null && r.acao.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.modulo != null && r.modulo.ToUpper().Contains(searchBy.ToUpper()) ||
                                               r.recuperacaoJudicial == true && r.recuperacaoJudicial.Equals(searchBy.ToUpper()) ||
                                               r.recuperacaoJudicial == false && r.recuperacaoJudicial.Equals(searchBy.ToUpper())
                                               );
                }

                var data2 = orderAscendingDirection ? _logListByParameter.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Asc).ToList() : _logListByParameter.AsQueryable().OrderByDynamic(orderCriteria, DtOrderDir.Desc).ToList();

                // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
                var filteredResultsCount = data2.Count();
                var totalResultsCount = _context.LogItems.Count();

                if (string.IsNullOrEmpty(searchBy))
                {

                    _logListByParameter.Where(item => item.pasta == pasta && item.contrato == contrato && item.tipoContrato == tipoContrato && item.recuperacaoJudicial == recuperacaoJudicial)
                                           .OrderByDescending(x => x.data);
                    var data3 = _logListByParameter.ToList();
                    int recordsTotal = _logListByParameter.Count();
                }

                if (getAll)
                {
                    return _logListByParameter.ToList();
                }
                else
                {
                    var data = _logListByParameter
                        .Skip(dtParameters.Start)
                        .Take(dtParameters.Length)
                        .ToList();

                    return new ObjectResult(new
                    {
                        draw = dtParameters.Draw,
                        recordsTotal = totalResultsCount,
                        recordsFiltered = filteredResultsCount,
                        data
                    });
                }
            }
            else
            {
                return this.GetLogItems(pasta, contrato, tipoContrato, pageSize, pageNumber, draw, getAll, recuperacaoJudicial);
            }
        }

        //PUT:      api/users/n
        [HttpPut]
        public ActionResult PutLogItem(List<Log> logList)
        {
            foreach (var log in logList)
            {
                _context.Entry(log).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }

        //DELETE:   api/users/n
        [HttpDelete("{id}")]
        public ActionResult<Log> DeleteLogItem(int id)
        {
            var logItem = _context.LogItems.Find(id);

            if (logItem == null)
            {
                return NotFound();
            }

            _context.LogItems.Remove(logItem);
            _context.SaveChanges();

            return logItem;
        }
    }
}