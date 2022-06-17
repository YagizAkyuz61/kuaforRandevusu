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
    public class TimesController : ControllerBase
    {
        private krDbContext _krDbContext;
        public TimesController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpGet("{id}")]
        [HttpGet]
        public IActionResult GetTimes(int id)
        {
            var times = from time in _krDbContext.TimesTable
                        join name in _krDbContext.UserTable on time.KId equals name.Id
                        orderby time.Time ascending
                        where time.KId == id
                        select new
                        {
                            Id = time.Id,
                            Name = name.Name,
                            Time = time.Time
                        };
            return Ok(times);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TimesClass timesObj)
        {
            _krDbContext.TimesTable.Add(timesObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
