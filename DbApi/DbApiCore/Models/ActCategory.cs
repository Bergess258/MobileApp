using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class ActCategory
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int CategoryId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Category Category { get; set; }
    }
}
