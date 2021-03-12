using System;
using Xunit;
using DbApiCore.Models;
using DbApiCore.Controllers;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

namespace XUnitTestProject1
{
    public class UnitTest1
    {

        [Fact]
        public async void TestPostActChat()
        {
            
            ActChat actChat = new ActChat() {UserId = 11, ActivityId = 78,Text = "Bruh",UserName="Testovoe" };
            DbContextOptionsBuilder<DBContx> contextOptions = new DbContextOptionsBuilder<DBContx>();
            contextOptions.UseNpgsql(DbApiCore.Properties.Resources.dbCon);
            DBContx db = new DBContx(contextOptions.Options);
            ActChatsController controller = new ActChatsController(db);
            await controller.PostActChat(actChat);
        }

        [Fact]
        public async void TestActivityDelete()
        {

            DbContextOptionsBuilder<DBContx> contextOptions = new DbContextOptionsBuilder<DBContx>();
            contextOptions.UseNpgsql(DbApiCore.Properties.Resources.dbCon);
            DBContx db = new DBContx(contextOptions.Options);
            ActivitiesController controller = new ActivitiesController(db);
            await controller.DeleteActivity(60);
        }

        [Fact]
        public async void TestCategoryDelete()
        {

            DbContextOptionsBuilder<DBContx> contextOptions = new DbContextOptionsBuilder<DBContx>();
            contextOptions.UseNpgsql(DbApiCore.Properties.Resources.dbCon);
            DBContx db = new DBContx(contextOptions.Options);
            CategoriesController controller = new CategoriesController(db);
            await controller.DeleteCategory(49);
        }

        [Fact]
        public async void TestUserDelete()
        {

            DbContextOptionsBuilder<DBContx> contextOptions = new DbContextOptionsBuilder<DBContx>();
            contextOptions.UseNpgsql(DbApiCore.Properties.Resources.dbCon);
            DBContx db = new DBContx(contextOptions.Options);
            UsersController controller = new UsersController(db);
            await controller.DeleteUser(11);
        }
    }
}
