using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

#nullable disable

namespace DbApiCore.Models
{
    public partial class User
    {
        public User()
        {
            ActAttendings = new HashSet<ActAttending>();
            ActChats = new HashSet<ActChat>();
            QuestTaskUsers = new HashSet<QuestTaskUser>();
            UserImgs = new HashSet<UserImg>();
            UserQuests = new HashSet<UserQuest>();
            UsersKpihistories = new HashSet<UsersKpihistory>();
        }

        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool MailConfirm { get; set; }
        public string Name { get; set; }
        public int Currency { get; set; }
        public int Thanks { get; set; }
        public char? Role { get; set; }
        public float EngPoints { get; set; }
        public DateTime LastEntry { get; set; }
        public float Bonus { get; set; }

        public virtual ICollection<ActAttending> ActAttendings { get; set; }
        public virtual ICollection<ActChat> ActChats { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUsers { get; set; }
        public virtual ICollection<UserImg> UserImgs { get; set; }
        public virtual ICollection<UserQuest> UserQuests { get; set; }
        public virtual ICollection<UsersKpihistory> UsersKpihistories { get; set; }
        public void AddKPI(DBContx db, float kpiToAdd)
        {
            EngPoints += kpiToAdd;
            db.UsersKpihistories.Add(new UsersKpihistory() { Date = DateTime.Today, Kpiadded = kpiToAdd, UserId = Id });
            db.Entry(this).State = EntityState.Modified;
            Company comp = db.Company.Find(1);
            comp.Kpi += kpiToAdd;
            db.Entry(this).State = EntityState.Modified;
            //db.SaveChanges(); Сейчас пока везде сохраняется и так
        }
    }
}
