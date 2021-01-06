using System;
using System.Collections.Generic;

#nullable disable

namespace ConsoleApp7.Models
{
    public partial class QuestTaskUser
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestTaskId { get; set; }
        public int Counter { get; set; }

        public virtual QuestTask QuestTask { get; set; }
        public virtual User User { get; set; }
    }
}
