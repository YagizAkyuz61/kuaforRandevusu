using Api.Data;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RSController : ControllerBase
    {
        private krDbContext _krDbContext;
        public RSController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }
        
        [HttpGet("{id}")]
        public IActionResult GetReservationDetail(int id)
        {
            var reservationResult = (from reservation in _krDbContext.ReservationTable
                                     join name in _krDbContext.UserTable on reservation.KId equals name.Id
                                     orderby reservation.Time ascending
                                     orderby reservation.Date ascending
                                     where reservation.Date >= DateTime.Today
                                     where reservation.KId == id
                                     select new
                                     {
                                         Id = reservation.Id,
                                         name.Name,
                                         CName = reservation.CName,
                                         Operation = reservation.Operation,
                                         Date = reservation.Date,
                                         Time = reservation.Time
                                     });
            return Ok(reservationResult);
        }
    }
}
