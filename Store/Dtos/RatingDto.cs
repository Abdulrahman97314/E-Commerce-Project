using System.ComponentModel.DataAnnotations;

namespace Store.APIs.Dtos
{
    public class RatingDto
    {
        public int ProductId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string Message { get; set; }
    }
}
