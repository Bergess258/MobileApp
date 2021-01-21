using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DBWebApi.Models
{
    public partial class Category
    {
        public Category()
        {
            ActCategories = new HashSet<ActCategory>();
        }

        public Category(string name):base()
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ActCategory> ActCategories { get; set; }
    }
}
