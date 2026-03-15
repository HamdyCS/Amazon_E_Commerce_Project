using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace ApiLayer.Controllers
{
    [Route("api/banners")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }
        [HttpGet("all", Name = "GetAllBanners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<BannerDto>>> GetAllBanners([FromQuery, Range(1, int.MaxValue)] int pageNumber, [FromQuery, Range(1, int.MaxValue)] int pageSize)
        {

            var banners = await _bannerService.GetAllBannersPagesAsync(pageNumber, pageSize);

            if (!banners.Any())
                return NotFound("Not found any banner.");
            return Ok(banners);
        }

        [AllowAnonymous]
        [HttpGet("all/active", Name = "GetAllActiveBanners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<BannerDto>>> GetAllActiveBanners()
        {

            var banners = await _bannerService.GetActiveBannersAsync();

            if (!banners.Any())
                return NotFound("Not found any banner.");
            return Ok(banners);
        }


        [HttpGet("{id}", Name = "GetBannerById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerDto>> GetBannerById(long id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0.");

            var banner = await _bannerService.GetBannerByIdAsync(id);
            return Ok(banner);

        }


        [HttpPost("", Name = "AddNewBanner")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerDto>> AddNewBanner([FromForm] CreateBannerDto createBannerDto)
        {

            var banner = await _bannerService.AddBannerAsync(createBannerDto);

            return CreatedAtRoute("GetBannerById", new { id = banner.Id }, banner);
  
        }


        [HttpPost("bulk",Name = "AddNewBanners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerDto>> AddNewBanners([FromForm] IEnumerable<CreateBannerDto> createBannerDtos)
        {

            var banners = await _bannerService.AddBannersAsync(createBannerDtos);

            return Ok(banners);
        }


        [HttpPut(Name = "UpdateBanner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerDto>> UpdateBanner([FromBody]UpdateBannerDto updateBannerDto)
        {

            var banners = await _bannerService.UpdateBannerAsync(updateBannerDto);

            return Ok(banners);
        }


        [HttpPut("range",Name = "UpdateBanners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerDto>> UpdateBanners([FromBody] IEnumerable< UpdateBannerDto>updateBannerDtos)
        {

            var banners = await _bannerService.UpdateBannersAsync(updateBannerDtos);

            return Ok(banners);
        }


        [HttpDelete("{id}",Name = "DeleteBanner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBanner([FromRoute]int id)
        {
            if (id <= 0)
                return BadRequest("Id must be greater than 0.");

            var isDeleted = await _bannerService.DeleteBannerByIdAsync(id);

           if(!isDeleted)
                return BadRequest("Failed to delete banner");

            return Ok(new
            {
                message = "Banner deleted successfully."
            });
        }


        [HttpDelete("active", Name = "DeleteActiveBanners")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteActiveBanners()
        {
            
            var isDeleted = await _bannerService.DeleteAllActiveBannersAsync();

            if (!isDeleted)
                return BadRequest("Failed to delete active banners");

            return Ok(new
            {
                message = "Banners deleted successfully."
            });
        }
    }
}
