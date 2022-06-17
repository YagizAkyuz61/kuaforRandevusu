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
    public class OperationController : ControllerBase
    {
        private krDbContext _krDbContext;
        public OperationController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpGet("{g}")]
        [HttpGet]
        public IActionResult GetOperation(string g)
        {
            var operations = from operation in _krDbContext.OperationTable
                             where operation.Gender == g
                             select new
                             {
                                 operation.Operation
                             };
            return Ok(operations);
        }
    }
}
