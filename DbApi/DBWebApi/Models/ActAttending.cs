using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class ActAttending
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int UserId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual User User { get; set; }
    }
}
