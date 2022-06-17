using Api.Data;
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
    public class LHOController : ControllerBase
    {
        private krDbContext _krDbContext;
        public LHOController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpGet("{id}")]
        [HttpGet]
        public IActionResult GetHOperation(int id)
        {
            var operations = from operation in _krDbContext.HOperationTable
                             where operation.KId == id
                             select new
                             {
                                 Id = operation.Id,
                                 KId = operation.KId,
                                 Operation = operation.Operation,
                                 Price = operation.Price
                             };
            return Ok(operations);
        }
    }
}
