using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}