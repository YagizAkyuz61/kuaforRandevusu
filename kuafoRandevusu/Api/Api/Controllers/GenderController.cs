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
    public class GenderController : ControllerBase
    {
        private readonly List<GenderClass> _gender = new List<GenderClass>()
        {
            new GenderClass() {Id = 1, Gender = "Kadın-Erkek"},
            new GenderClass() {Id = 2, Gender = "Kadın"},
            new GenderClass() {Id = 3, Gender = "Erkek"},
            new GenderClass() {Id = 4, Gender = "Çocuk"},
            new GenderClass() {Id = 5, Gender = "Hayvan"}
        };

        public IQueryable<GenderClass> GetGender()
        {
            return _gender.AsQueryable();
        }
    }
}
