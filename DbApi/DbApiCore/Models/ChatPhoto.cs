using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class ChatPhoto
    {
        public int Id { get; set; }
        public int ActChatId { get; set; }
        public byte[] Photo { get; set; }

        public virtual ActChat ActChat { get; set; }
    }
}
