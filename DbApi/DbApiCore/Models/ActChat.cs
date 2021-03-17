using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class ActChat
    {
        public ActChat()
        {
            ChatPhotos = new HashSet<ChatPhoto>();
        }

        public int Id { get; set; }
        public int Thanks { get; set; }
        public string Text { get; set; }
        public int? UserId { get; set; }
        public int ActivityId { get; set; }
        public string UserName { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ChatPhoto> ChatPhotos { get; set; }
    }
}
