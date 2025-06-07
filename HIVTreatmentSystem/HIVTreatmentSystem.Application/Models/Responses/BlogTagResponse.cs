using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class BlogTagResponse
    {
        public int BlogTagId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }
}
