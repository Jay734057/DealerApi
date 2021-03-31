using Newtonsoft.Json;

namespace TaskV3.Core.Models
{
    public class Stock
    {
        public int Amount { get; set; }

        public int DealerId { get; set; }
        [JsonIgnore]
        public Dealer Dealer { get; set; }

        public int CarId { get; set; }
        [JsonIgnore]
        public Car Car { get; set; }

    }
}
