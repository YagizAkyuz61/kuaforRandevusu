using System;
using System.Collections.Generic;
using System.Text;

namespace Mobil.Model
{
    public class ReservationClass
    {
        public int Id { get; set; }
        public int KId { get; set; }
        public int CId { get; set; }
        public string CName { get; set; }
        public string Operation { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}
