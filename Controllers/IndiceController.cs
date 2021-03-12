using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using calculadora_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace calculadora_api.Controllers
{
    [Route("api/indices")]
    [ApiController]
    public class IndiceController : ControllerBase
    {
        private readonly UserContext _context;

        public IndiceController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Indice>> GetIndiceItemsParameter([FromQuery] string indice, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 0, [FromQuery] int draw = 1, [FromQuery] bool getAll = false)
        {
            IQueryable<Indice> _indiceListByParameter = _context.IndiceItems.Where(item => item.indice == indice).OrderByDescending(x => x.data);
            int recordsTotal = _indiceListByParameter.Count();

            if (getAll)
            {
                return _indiceListByParameter.ToList();
            }
            else
            {
                var data = _indiceListByParameter
                       .Skip(pageSize * pageNumber)
                       .Take(pageSize)
                       .ToList();

                return new ObjectResult(new { draw, recordsTotal, data });
            }

        }


        [HttpGet("byDate")]
        public ActionResult<Indice> GetIndiceItemsByDate([FromQuery] string indice, [FromQuery] DateTime data)
        {
            return _context.IndiceItems
                        .Where(item => item.indice == indice)
                        .Where(item => item.data == data)
                        .OrderByDescending(x => x.data)
                        .Single();
        }


        [HttpGet("{id}")]
        public ActionResult<Indice> IndiceItem(int id)
        {
            var indiceItem = _context.IndiceItems.Find(id);

            if (indiceItem == null)
            {
                return NotFound();
            }

            return indiceItem;
        }


        [HttpPost]
        public ActionResult PostIndiceItem(List<Indice> indiceList)
        {
            foreach (var indice in indiceList)
            {
                _context.IndiceItems.Add(indice);
                _context.SaveChanges();
            }
            return NoContent();
        }


        [HttpPut]
        public ActionResult PutIndiceItem(List<Indice> indiceList)
        {
            foreach (var indice in indiceList)
            {
                _context.Entry(indice).State = EntityState.Modified;
                _context.SaveChanges();
            }

            return NoContent();
        }


        [HttpDelete("{id}")]
        public ActionResult<Indice> DeleteIndiceItem(int id)
        {
            var indiceItem = _context.IndiceItems.Find(id);

            if (indiceItem == null)
            {
                return NotFound();
            }

            _context.IndiceItems.Remove(indiceItem);
            _context.SaveChanges();

            return indiceItem;
        }

        
        public double getIndiceDataBase(string indiceDB, DateTime dataBase, Object formDefaultValues) {
            if (indiceDB == null || dataBase == null) {
                return 1;
            }

            switch (indiceDB) {
            case "Encargos Contratuais %":
                dynamic dynamic = JsonConvert.DeserializeObject(formDefaultValues.ToString());
                return Convert.ToDouble(dynamic["formIndiceEncargos"].ToString());
            default:
                return GetIndiceItemsByDate(indiceDB, dataBase).Value.valor;
            }
        }
    }
}