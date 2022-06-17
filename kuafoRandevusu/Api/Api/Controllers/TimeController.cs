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
    public class TimeController : ControllerBase
    {
        private readonly List<TimeClass> _time = new List<TimeClass>()
        {
            new TimeClass() {Time = "08:00"},
            new TimeClass() {Time = "08:30"},
            new TimeClass() {Time = "08:45"},
            new TimeClass() {Time = "09:00"},
            new TimeClass() {Time = "09:30"},
            new TimeClass() {Time = "09:45"},
            new TimeClass() {Time = "10:00"},
            new TimeClass() {Time = "10:30"},
            new TimeClass() {Time = "10:45"},
            new TimeClass() {Time = "11:00"},
            new TimeClass() {Time = "11:30"},
            new TimeClass() {Time = "11:45"},
            new TimeClass() {Time = "12:00"},
            new TimeClass() {Time = "12:30"},
            new TimeClass() {Time = "12:45"},
            new TimeClass() {Time = "13:00"},
            new TimeClass() {Time = "13:30"},
            new TimeClass() {Time = "13:45"},
            new TimeClass() {Time = "14:30"},
            new TimeClass() {Time = "14:45"},
            new TimeClass() {Time = "15:00"},
            new TimeClass() {Time = "15:30"},
            new TimeClass() {Time = "15:45"},
            new TimeClass() {Time = "16:00"},
            new TimeClass() {Time = "16:30"},
            new TimeClass() {Time = "16:45"},
            new TimeClass() {Time = "16:00"},
            new TimeClass() {Time = "16:30"},
            new TimeClass() {Time = "16:45"},
            new TimeClass() {Time = "17:00"},
            new TimeClass() {Time = "17:30"},
            new TimeClass() {Time = "17:45"},
            new TimeClass() {Time = "18:00"},
            new TimeClass() {Time = "18:30"},
            new TimeClass() {Time = "18:45"},
            new TimeClass() {Time = "19:00"},
            new TimeClass() {Time = "19:30"}
        };

        public IQueryable<TimeClass> GetTime()
        {
            return _time.AsQueryable();
        }
    }
}
