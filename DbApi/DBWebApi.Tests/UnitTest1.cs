﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DbWebApi.Tests
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

        [TestMethod]
        public void TestPostUser()
        {
            User testObj = new User() {Name = "Test",Mail = "Test",Password = "Bruh" };
            var qt = new UsersController();
            var result = qt.PostUser(testObj);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestPostActivityNoCategory()
        {
            Activity testObj = new Activity() { Name = "Test"};
            var qt = new ActivitiesController();
            var result = qt.PostActivity(testObj);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestPostActivityWithCategory()
        {
            ActWithCatPost testObj = new ActWithCatPost("TestCategories222");
            testObj.categories = new Category[] { new Category("Проверочное") { Id=48},new Category("Обязательно") { Id = 47 } };
            var qt = new ActivitiesController();
            var result = qt.PostActivity(testObj);
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void TestAttending()
        {
            var qt = new ActivitiesController();
            var result = qt.GetActivity(18,1);
            Assert.IsNotNull(result);
        }
    }
}
