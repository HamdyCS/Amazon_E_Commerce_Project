using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestController(AppDbContext context)
             
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Get()
        {

            return Ok(_context.People.ToList());
        }
    }
}
