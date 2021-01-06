using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class UserImg
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? Img { get; set; }

        public virtual User User { get; set; }
    }
}
