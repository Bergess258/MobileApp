using System;
using System.Collections.Generic;


namespace DBWebApi.Models
{
    public partial class ActCategory
    {
        public ActCategory()
        {
            Activities = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public int ActivityId { get; set; }
        public int CategoryId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
