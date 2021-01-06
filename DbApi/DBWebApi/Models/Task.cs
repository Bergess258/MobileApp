using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class Task
    {
        public Task()
        {
            QuestTasks = new HashSet<QuestTask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<QuestTask> QuestTasks { get; set; }
    }
}
