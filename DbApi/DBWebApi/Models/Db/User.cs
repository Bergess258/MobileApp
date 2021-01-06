using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleApp7.Models
{
    public partial class User
    {
        public User()
        {
            ActAttendings = new HashSet<ActAttending>();
            QuestTaskUsers = new HashSet<QuestTaskUser>();
            UserImgs = new HashSet<UserImg>();
            UserQuests = new HashSet<UserQuest>();
        }

        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool MailConfirm { get; set; }
        public string Name { get; set; }
        public int Currency { get; set; }
        public int Thanks { get; set; }
        public string Role { get; set; }
        public int EngPoints { get; set; }

        public virtual ICollection<ActAttending> ActAttendings { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUsers { get; set; }
        public virtual ICollection<UserImg> UserImgs { get; set; }
        public virtual ICollection<UserQuest> UserQuests { get; set; }
    }
}
