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
    public class CommentsController : ControllerBase
    {
        private readonly List<CommentsClass> _comment = new List<CommentsClass>()
        {
            new CommentsClass() {Comment = "Çok iyi ve çok ilgili"},
            new CommentsClass() {Comment = "Çok iyi"},
            new CommentsClass() {Comment = "Tavsiye ederim"},
            new CommentsClass() {Comment = "Dakik kuaför"},
            new CommentsClass() {Comment = "İyi"},
            new CommentsClass() {Comment = "Orta"},
            new CommentsClass() {Comment = "Kötü"},
            new CommentsClass() {Comment = "Zamanında almadı"},
            new CommentsClass() {Comment = "Çok kötü"}
        };

        public IQueryable<CommentsClass> GetComments()
        {
            return _comment.AsQueryable();
        }
    }
}
