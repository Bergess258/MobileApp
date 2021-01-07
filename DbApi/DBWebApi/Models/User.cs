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
        public string Role { get; set; }
        //Склироз мешает сори(если вспомню поправлю)
        public int EngPoints { get; set; }

        public virtual ICollection<ActAttending> ActAttendings { get; set; }
        public virtual ICollection<QuestTaskUser> QuestTaskUsers { get; set; }
        //можно не читать полностью <3
        //отдельная хрень на картинку(так вроде лучше работать должно и не будет нужная инфа подгружаться "долго"(в кавычках потому что я хз сколько это времени займет в обоих случаях))
        public virtual ICollection<UserImg> UserImg { get; set; }
        public virtual ICollection<UserQuest> UserQuests { get; set; }
    }
}
