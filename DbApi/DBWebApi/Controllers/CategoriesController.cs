using DBWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBWebApi.Controllers
{
    public class CategoriesController : ApiController
    {
        private DBContx db = new DBContx();

        public IQueryable<string> Get()
        {
            return db.Categories.Select(x=>x.Name);
        }

    }
}
