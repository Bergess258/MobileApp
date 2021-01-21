using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class Category
    {
        public Category()
        {
            ActCategories = new HashSet<ActCategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ActCategory> ActCategories { get; set; }
    }
}
