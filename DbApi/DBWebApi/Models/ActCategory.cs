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
        public string Name { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
