using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using DataAccessLayer.Contracks;
using DataAccessLayer.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPersonService _personServices;

        public TestController(IPersonService personServices)
             
        {
        }

        [HttpGet]
        public async Task<IActionResult>GetById()
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PersonDto personDto)
        {
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> update(PersonDto personDto)
        {
            return Ok(personDto);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(PersonDto personDto)
        {
            return Ok("Ok");
        }
    }
}
