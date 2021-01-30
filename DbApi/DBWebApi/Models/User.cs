using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DBWebApi.Models
{
    public partial class User
    {
        public User()
        {
            ActAttendings = new HashSet<ActAttending>();
            QuestTaskUsers = new HashSet<QuestTaskUser>();
            UserImg = new HashSet<UserImg>();
            UserQuests = new HashSet<UserQuest>();
            UsersKpihistories = new HashSet<UsersKpihistory>();
        }
        public int Id { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public bool MailConfirm { get; set; }
        public string Name { get; set; }
        //Бабки в магазе
        public int Currency { get; set; }
        //Уже забыл зачем. Вроде как репутация, можно просто кого-то благодарить. Нлорм тема
        public int Thanks { get; set; }
        //Для админов
        public char? Role { get; set; }
        //Теперь это очки kpi
        public float EngPoints { get; set; }

        public virtual ICollection<ActAttending> ActAttendings { get; set; }
        public virtual ICollection<ActChat> ActChat { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUsers { get; set; }
        //можно не читать полностью <3
        //отдельная хрень на картинку(так вроде лучше работать должно и не будет нужная инфа подгружаться "долго"(в кавычках потому что я хз сколько это времени займет в обоих случаях))
        public virtual ICollection<UserImg> UserImg { get; set; }
        public virtual ICollection<UserQuest> UserQuests { get; set; }
        public virtual ICollection<UsersKpihistory> UsersKpihistories { get; set; }

        public void AddKPI(DBContx db,float kpiToAdd)
        {
            EngPoints += kpiToAdd;
            db.UsersKpihistories.Add(new UsersKpihistory() { Date = DateTime.Today, Kpiadded = kpiToAdd, UserId = Id });
            db.Entry(this).State = EntityState.Modified;
            //db.SaveChanges(); Сейчас пока везде сохраняется и так
        }
    }
}
