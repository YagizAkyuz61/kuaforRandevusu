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
    public class CSController : ControllerBase
    {
        private krDbContext _krDbContext;
        public CSController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpGet("{id}")]
        public IActionResult GetCuReservationDetail(int id)
        {
            var reservationResult = (from reservation in _krDbContext.ReservationTable
                                     join userName in _krDbContext.CustomerTable on reservation.CId equals userName.Id
                                     join name in _krDbContext.UserTable on reservation.KId equals name.Id
                                     orderby reservation.Time ascending
                                     orderby reservation.Date ascending                                     
                                     where reservation.Date >= DateTime.Today
                                     where reservation.CId == id
                                     select new
                                     {
                                         Id = reservation.Id,
                                         name.Name,
                                         userName.UserName,
                                         CName = reservation.CName,
                                         Operation = reservation.Operation,
                                         Date = reservation.Date,
                                         Time = reservation.Time
                                     });
            return Ok(reservationResult);
        }
    }
}
