﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DBWebApi.Data
{
    public class DBWebApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public DBWebApiContext() : base("name=DBWebApiContext")
        {
        }

        public System.Data.Entity.DbSet<DBWebApi.Models.Category> Categories { get; set; }
    }
}
