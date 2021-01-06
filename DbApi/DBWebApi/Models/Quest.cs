using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class Quest
    {
        public Quest()
        {
            QuestTasks = new HashSet<QuestTask>();
            UserQuests = new HashSet<UserQuest>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? IconId { get; set; }
        public int Currency { get; set; }

        public virtual ICollection<QuestTask> QuestTasks { get; set; }
        public virtual ICollection<UserQuest> UserQuests { get; set; }
    }
}
