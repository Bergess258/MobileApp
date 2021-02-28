using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class Company
    {
        public int Id { get; set; }
        public float Kpi { get; set; }
        public string Name { get; set; }
    }
}
