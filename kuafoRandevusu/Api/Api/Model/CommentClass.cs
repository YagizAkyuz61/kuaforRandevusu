using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model
{
    public class CommentClass
    {
        public int Id { get; set; }
        public int KId { get; set; }
        public string CName { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
