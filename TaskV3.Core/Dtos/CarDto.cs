using System.ComponentModel.DataAnnotations;

namespace TaskV3.Core.Dtos
{
    public class CarDto
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public short Year { get; set; }
    }
}
