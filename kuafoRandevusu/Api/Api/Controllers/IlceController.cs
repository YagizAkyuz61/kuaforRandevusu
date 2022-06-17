using Api.Data;
using Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IlceController : ControllerBase
    {
        private krDbContext _krDbContext;
        public IlceController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] IlceClass ılce)
        {
            _krDbContext.IlceTable.Add(ılce);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetIlce()
        {
            var ilceResult = from ılce in _krDbContext.IlceTable
                             select new
                             {
                                 Id = ılce.Id,
                                 IName = ılce.IName,
                             };
            return Ok(ilceResult);
        }

        [HttpGet("{id}")]
        public IActionResult GetIlces(int id)
        {
            var ilceResult = (from ilce in _krDbContext.IlceTable
                              where ilce.CId == id
                              select new
                              {
                                  Id = ilce.Id,
                                  CId = ilce.CId,
                                  IName = ilce.IName
                              }).DefaultIfEmpty();
            return Ok(ilceResult);
        }
    }
}
