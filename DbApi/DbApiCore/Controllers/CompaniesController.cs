using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DbApiCore.Models;

namespace DbApiCore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly DBContx _context;

        public CompaniesController(DBContx context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<Company> GetCompany()
        {
            return await _context.Company.FindAsync(1);
        }

    }
}
