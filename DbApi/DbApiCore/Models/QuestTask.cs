using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class QuestTask
    {
        public QuestTask()
        {
            QuestTaskUsers = new HashSet<QuestTaskUser>();
        }

        public int Id { get; set; }
        public int QuestId { get; set; }
        public int TaskId { get; set; }
        public int CountToComplete { get; set; }
        public int ComplOneReward { get; set; }

        public virtual Quest Quest { get; set; }
        public virtual Task Task { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUsers { get; set; }
    }
}
