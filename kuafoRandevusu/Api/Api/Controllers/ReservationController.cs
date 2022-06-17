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
    public class ReservationController : ControllerBase
    {
        private krDbContext _krDbContext;
        public ReservationController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] ReservationClass reservationObj)
        {
            _krDbContext.ReservationTable.Add(reservationObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet]
        public IActionResult GetReservations()
        {
            var reservations = from reservation in _krDbContext.ReservationTable
                               join name in _krDbContext.UserTable on reservation.KId equals name.Id
                               orderby reservation.Time ascending
                               select new
                               {
                                   Id = reservation.Id,
                                   name.Name,
                                   CName = reservation.CName,
                                   Operation = reservation.Operation,
                                   Date = reservation.Date,
                                   Time = reservation.Time
                               };
            return Ok(reservations);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ReservationClass reservation)
        {
            var reservations = _krDbContext.ReservationTable.Find(id);
            if (reservations == null)
            {
                return NotFound("Hata");
            }
            else
            {
                reservations.KId = reservation.KId;
                reservations.CId = reservation.CId;
                reservations.CName = reservation.CName;
                reservations.Operation = reservation.Operation;
                reservations.Date = reservation.Date;
                reservations.Time = reservation.Time;
                _krDbContext.SaveChanges();
                return Ok("Güncelleme başarılı.");
            }
        }

        // GET api/<ReservationController>/5
        [HttpGet("{id}")]
        public ReservationClass Get(int id)
        {
            var reservation = _krDbContext.ReservationTable.Find(id);
            return reservation;
        }
    }
}
