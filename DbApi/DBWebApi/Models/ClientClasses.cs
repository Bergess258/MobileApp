using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBWebApi.Models
{

    public class QuestWithTasks : Quest
    {
        public bool done { get; set; }
        public TaskWithCounter[] tasks { get; set; }
        public QuestWithTasks(Quest q,int tasksCount,bool status)
        {
            Name = q.Name;
            Currency = q.Currency;
            IconId = q.IconId;
            tasks = new TaskWithCounter[tasksCount];
            done = status;
        }
        public Quest GetQuest()
        {
            return new Quest() { Id = Id, Name = Name, Currency = Currency, IconId = IconId };
        }
    }
    public class TaskWithCounter:Task
    {
        public int countToComplete { get; set; }
        public int counter { get; set; }
        public TaskWithCounter(Task task,int CurrentCounter,int CounterToComplete)
        {
            Name = task.Name;
            counter = CurrentCounter;
            countToComplete = CounterToComplete;
        }
    }
    public class UserQuestWithTasks : QuestWithTasks
    {
        public User[] users { get; set; }
        public UserQuestWithTasks(Quest q, int tasksCount, bool status):base(q,tasksCount,status)
        {

        }
    }

}