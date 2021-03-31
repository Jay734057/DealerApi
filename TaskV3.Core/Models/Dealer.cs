using System.Collections.Generic;

namespace TaskV3.Core.Models
{
    public class Dealer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<Stock> Stocks{ get; set; }
    }
}
