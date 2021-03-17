using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class UserImg
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[] Img { get; set; }

        public virtual User User { get; set; }
    }
}
