using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/brands")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class BrandsController : ControllerBase
    {
        private readonly IBrandService _BrandService;

        public BrandsController(IBrandService IBrandService)
        {
            this._BrandService = IBrandService;
        }

        [HttpGet("{Id}", Name = "GetBrandById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetBrandById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");

            try
            {
                var productCategoryDto = await _BrandService.FindByIdAsync(Id);

                if (productCategoryDto == null) return NotFound($"Not found brand. Id = {Id}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-ar/{NameAr}", Name = "GetBrandByNameAr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetBrandByNameAr(string NameAr)
        {
            if (string.IsNullOrEmpty(NameAr)) return BadRequest("NameAr is null or empty");

            try
            {
                var productCategoryDto = await _BrandService.FindByNameArAsync(NameAr);

                if (productCategoryDto == null) return NotFound($"Not found brand. NameAR = {NameAr}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-en/{NameEn}", Name = "GetBrandByNameEn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetBrandByNameEn(string NameEn)
        {
            if (string.IsNullOrEmpty(NameEn)) return BadRequest("NameEn is null or empty");

            try
            {
                var productCategoryDto = await _BrandService.FindByNameEnAsync(NameEn);

                if (productCategoryDto == null) return NotFound($"Not found brand. NameEn = {NameEn}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetAllBrands()
        {

            try
            {
                var brandsDtos = await _BrandService.GetAllAsync();

                if (brandsDtos == null || !brandsDtos.Any())
                    return NotFound($"Didnot find any brnad");

                return Ok(brandsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("count", Name = "GetCountOfBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> GetCountOfBrands()
        {
            try
            {
                var count = await _BrandService.GetCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-paged", Name = "GetPagedOfBrands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetPagedOfBrands([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0");

            try
            {
                var brandsDtos = await _BrandService.GetPagedDataAsync(pageNumber, pageSize);

                if (brandsDtos == null || !brandsDtos.Any())
                    return NotFound($"Didnot find any brand");

                return Ok(brandsDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "Addbrand")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductCategoryDto>> Addbrand(BrandDto brandDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewBrandDto = await _BrandService.AddAsync(brandDto, UserId);

                if (NewBrandDto == null) return BadRequest("Cannot add new brand");

                return CreatedAtRoute("GetBrandById", new { Id = NewBrandDto.Id }, NewBrandDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfBrands")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> AddRangeOfBrands(IEnumerable<BrandDto> brnadsDtos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewBrandsDtos = await _BrandService.AddRangeAsync(brnadsDtos, UserId);

                if (NewBrandsDtos == null || !NewBrandsDtos.Any()) return BadRequest("Cannot add new brands");

                return Ok("Added new product Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateBrandById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateBrandById([FromRoute] long Id, [FromBody] BrandDto brandDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var IsBrandUpdated = await _BrandService.UpdateByIdAsync(Id, brandDto);

                if (!IsBrandUpdated) return BadRequest("Cannot Update brand");

                return Ok("Updated brand successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteBrandById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteBrandById([FromRoute] long Id)
        {
            if (Id < 0) return BadRequest("Id must be bigger than zero");

            try
            {
                var IsDeleted = await _BrandService.DeleteByIdAsync(Id);

                if (!IsDeleted) return BadRequest("Cannot Delete brand");

                return Ok("Deleted brand successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}
