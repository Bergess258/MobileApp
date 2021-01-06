﻿using System;
using System.Collections.Generic;
using NpgsqlTypes;

#nullable disable

namespace ConsoleApp7.Models
{
    public partial class Activity
    {
        public Activity()
        {
            ActAttendings = new HashSet<ActAttending>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public NpgsqlRange<DateTime>? Date { get; set; }

        public virtual ActCategory Category { get; set; }
        public virtual ICollection<ActAttending> ActAttendings { get; set; }
    }
}
