using System;
using System.Collections.Generic;

namespace WebPruebaSofttek.Models
{
    public partial class Tbsaleasecom
    {
        public int Id { get; set; }
        public string Productname { get; set; }
        public int? Cnt { get; set; }
        public decimal? Priceunit { get; set; }
        public decimal? Pricefull { get; set; }
        public DateTime? Datefull { get; set; }
    }
}
