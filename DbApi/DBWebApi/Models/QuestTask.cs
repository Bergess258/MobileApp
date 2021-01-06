using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class QuestTask
    {
        public QuestTask()
        {
            QuestTaskUser = new HashSet<QuestTaskUser>();
        }

        public int Id { get; set; }
        public int QuestId { get; set; }
        public int TaskId { get; set; }
        public int CountToComplete { get; set; }
        //За выполнение одного раза из всех дадут столько-то
        public int ComplOneReward { get; set; }

        public virtual Quest Quest { get; set; }
        public virtual Task Task { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUser { get; set; }
    }
}
