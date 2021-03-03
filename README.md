# WebApi
Три страницы. Юзер, Квесты и Мероприятия.  
Работает на данный момент с любого компа локально.  
База уже на хостинге и все норм вроде.  
Есть файл с клиентскими классами для квестов(ClientClasses.cs)  
Мб, не понятно будет с ними, делайте свои, мне без разницы.  
## Юзер
Все стандартное типо гет, пут и все такое  
Чтобы залогитниться нужно прописать ссылку на хостинг(пока не разворачивал)(далее просто host)/Users?mail=мыло&pass=пароль  
Пример  
host/Users?mail=parshakov-00@mail.ru&pass=7V7a96Z9g8FxL269OdIBCRdx  
Пароль зашифрован уже. Мыло наверн стоит тоже, но я не уверен.  
Чтобы зарегать просто юзайте пост запрос и чтобы изменить пут. Как и в любом диругом крч.  
Отправить благодарность юзеру юзайте Users/SendThanks/айди юзера, которому посылаем
## Квесты(ОТключены)
Тоже все стандартное  
Немного про структуру квестов и тасков  
У квестов есть свое название и своя награда. Также есть таски, которые надо выполнить, чтобы завершить квест. Таски имеют название и награду за выполение одного раза(например, сходить на мероприятие 7 раз и при посещении одного мероприятия дается награда. За посещение 7 раз ничего не дается на данный момент, но это сделать изи. Не заморачивался пока с этим т.к. не думал как читать KPI и все такое).  
Добавлено два метода  
Добавление квестов с тасками - это вызов метода UsersQuestWithTasks т.е. Quests/UsersQuestWithTasks юзается класс QuestWithTasks  
Добавление квестов с тасками и привязкой к людям, чтобы сразу выполнялись. Quests/UsersQuestWithTasks юзается класс UserQuestWithTasks  
## Мероприятия
Все стандартное.
Добавлена возможность добавлять мероприятие с категориями. По ссылке Activities/WithCat в пост кидаем класс ActWithCatPost(находится в клиентских классах)
Новые категории должны подаваться с id 0(просто не назначать).  
Учет посещений Activities/id(айди мероприятия вместо id)?userid=айди юзера  
Просмотр посещение Activities/Attending/id(айди мероприятия)
Удаление посещения Activities/DelAttending/id(айди мероприятия вместо id)?userid=айди юзера  
Удаление посещения Activities/DelAtt/id(айди посещения) //Предпочтительно  
Получить категории мероприятия Activities/Categories/{id}
## Чат мероприятий
Все стандартно кроме  
/ActChats/id(мероприятия) возвращает чат мероприятия
## Категории
Просто запрос по ссылке Categories теперь возвращает Id и Name
Удаление стандартное.
## Компания 
По ссылке Company можно получить суммарное kpi компании за все время
