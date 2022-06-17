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
    public class CommentController : ControllerBase
    {
        private krDbContext _krDbContext;
        public CommentController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        [HttpGet("{id}")]
        public IActionResult GetComment(int id)
        {
            var comments = (from comment in _krDbContext.CommentsTable
                            join name in _krDbContext.UserTable on comment.KId equals name.Id
                            orderby comment.Date ascending
                            where comment.KId == id
                            select new
                            {
                                Id = comment.Id,
                                name.Name,
                                CName = comment.CName,
                                Comment = comment.Comment,
                                Date = comment.Date,
                            });
            return Ok(comments);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CommentClass commentObj)
        {
            _krDbContext.CommentsTable.Add(commentObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
