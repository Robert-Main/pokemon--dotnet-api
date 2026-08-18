using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos
{
    public class ReviewResponseDtos
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int Rating { get; set; }
        public Reviewer? Reviewer { get; set; }
    }
}
