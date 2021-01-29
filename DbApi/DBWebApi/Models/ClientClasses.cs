using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBWebApi.Models
{
    #region Quests

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
    #endregion

    #region Activities
    public class ActWithCat
    {
        public string name { get; set; }
        public DateTime? startD { get; set; }
        public DateTime? endD { get; set; }
        public string desc { get; set; }
        public ActWithCat()
        {

        }
        public ActWithCat(Activity activity)
        {
            name = activity.Name;
            startD = activity.StartD;
            endD = activity.EndD;
            desc = activity.Description;
        }
        public static implicit operator Activity(ActWithCat b) => new Activity() { Name = b.name, StartD = b.startD, EndD = b.endD,Description = b.desc };
    }
    public class ActWithCatGet:ActWithCat
    {
        public string[] categories;
        public ActWithCatGet(Activity activity):base(activity)
        {

        }

        public ActWithCatGet(Activity activity, string[] cats):base(activity)
        {
            categories = cats;
        }

    }

    public class ActWithCatPost: ActWithCat
    {
        public ActWithCatPost()
        {

        }
        public ActWithCatPost(string name)
        {
            base.name = name;
        }
        public ActWithCatPost(Activity activity) : base(activity)
        {

        }
        public Category[] categories;
        public ActWithCatPost(Activity activity, Category[] cats) : base(activity)
        {
            categories = cats;
        }

        public static implicit operator Activity(ActWithCatPost b) => new Activity() { Name = b.name, StartD = b.startD, EndD = b.endD, Description = b.desc };

    }

    #endregion

    #region Cat
    public class CatWithId
    {
        public int Id;
        public string Name;

        public CatWithId(int id,string name)
        {
            Id = id;
            Name = name;
        }
    }
    #endregion
}