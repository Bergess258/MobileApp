using DBWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DBWebApi.Controllers
{
    public class DuplicatesController : ApiController
    {
        private DBContx db = new DBContx();
        // GET: Duplicates
        public IHttpActionResult GetDuplicate()
        {
            //IGrouping<string,Category>[] categories = db.Categories.GroupBy(x => x.Name).Where(x => x.Count() > 1);
            //foreach (var item in categories)
            //{

            //}
            return Ok();
        }
    }
}