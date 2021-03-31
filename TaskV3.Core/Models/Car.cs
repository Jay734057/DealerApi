using System.Collections.Generic;

namespace TaskV3.Core.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public short Year { get; set; }
        public ICollection<Stock> Stocks{ get; set; }
    }
}
