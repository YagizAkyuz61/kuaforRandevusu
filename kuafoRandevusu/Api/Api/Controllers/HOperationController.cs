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
    public class HOperationController : ControllerBase
    {
        private krDbContext _krDbContext;
        public HOperationController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] HOperationClass operationObj)
        {
            _krDbContext.HOperationTable.Add(operationObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{id}")]
        [HttpGet]
        public IActionResult GetHOperation(int id)
        {
            var operations = from operation in _krDbContext.HOperationTable
                             join name in _krDbContext.UserTable on operation.KId equals name.Id
                             where operation.KId == id
                             select new
                             {
                                 Id = operation.Id,
                                 name.Name,
                                 Operation = operation.Operation + operation.Price
                               };
            return Ok(operations);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] HOperationClass operation)
        {
            var operations = _krDbContext.HOperationTable.Find(id);
            if (operations == null)
            {
                return NotFound("Hata");
            }
            else
            {
                operations.KId = operation.KId;
                operations.Operation = operation.Operation;
                operations.Price = operation.Price;
                _krDbContext.SaveChanges();
                return Ok("Güncelleme başarılı.");
            }
        }
    }
}
