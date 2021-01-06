using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DBWebApi.Controllers;
using DBWebApi.Models;

namespace DBWebApi.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestUsersQuestWithTasks()
        {
            Quest quest = new Quest() { Name = "", Currency = 50 };
            QuestWithTasks testObj = new QuestWithTasks(quest, 1, false);
            testObj.tasks = new TaskWithCounter[] { new TaskWithCounter(new Task() { Name = "Напишите ваш первый комментарий" }, 0, 1) };
            //testObj.users = new User[] { new User() { Id =1} };
            var qt = new QuestsController();
            var result = qt.QuestWithTasks(testObj);
            Assert.IsNotNull(result);
        }
    }
}
