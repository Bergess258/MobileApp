using System;
using System.Collections.Generic;


namespace DBWebApi.Models
{
    public partial class UsersKpihistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Kpiadded { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
    }
}
