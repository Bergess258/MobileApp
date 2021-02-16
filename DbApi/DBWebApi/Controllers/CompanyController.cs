using DBWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DBWebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private DBContx db = new DBContx();

        public Company GetCompany()
        {
            return db.Company.Find(1);
        }
    }
}
