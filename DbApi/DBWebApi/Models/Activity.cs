using System;
using System.Collections.Generic;
using NpgsqlTypes;


namespace DBWebApi.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ActAttendings = new HashSet<ActAttending>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartD { get; set; }
        public DateTime? EndD { get; set; }

        public virtual ICollection<ActAttending> ActAttendings { get; set; }
        public virtual ICollection<ActCategory> ActCategories { get; set; }
        public virtual ICollection<ActChat> ActChat { get; set; }
    }
}
