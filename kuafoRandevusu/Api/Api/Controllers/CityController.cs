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
    public class CityController : ControllerBase
    {
        private readonly List<CityClass> _city = new List<CityClass>()
        {
            new CityClass() {Id = 1, Name = "Adana"},
            new CityClass() {Id = 2, Name = "Ankara"},
            new CityClass() {Id = 3, Name = "Antalya"},
            new CityClass() {Id = 4, Name = "Bursa"},
            new CityClass() {Id = 5, Name = "Eskişehir"},
            new CityClass() {Id = 6, Name = "İstanbul"},
            new CityClass() {Id = 7, Name = "İzmir"},
            new CityClass() {Id = 8, Name = "Samsun"},
            new CityClass() {Id = 9, Name = "Trabzon"}
        };

        public IQueryable<CityClass> GetCity()
        {
            return _city.AsQueryable();
        }
    }
}