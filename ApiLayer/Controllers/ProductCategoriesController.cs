using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/product-categories")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ProductCategoriesController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoriesController(IProductCategoryService productCategoryService)
        {
            this._productCategoryService = productCategoryService;
        }

        [HttpGet("{Id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");

            try
            {
                var productCategoryDto = await _productCategoryService.FindByIdAsync(Id);

                if (productCategoryDto == null) return NotFound($"Not found product category. Id = {Id}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-ar/{NameAr}", Name = "GetByNameAr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetByNameAr(string NameAr)
        {
            if (string.IsNullOrEmpty(NameAr)) return BadRequest("NameAr is null or empty");

            try
            {
                var productCategoryDto = await _productCategoryService.FindByNameArAsync(NameAr);

                if (productCategoryDto == null) return NotFound($"Not found product category. NameAR = {NameAr}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-en/{NameEn}", Name = "GetByNameEn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetByNameEn(string NameEn)
        {
            if (string.IsNullOrEmpty(NameEn)) return BadRequest("NameEn is null or empty");

            try
            {
                var productCategoryDto = await _productCategoryService.FindByNameEnAsync(NameEn);

                if (productCategoryDto == null) return NotFound($"Not found product category. NameEn = {NameEn}");

                return Ok(productCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetAllProductCategories()
        {

            try
            {
                var productCategoriesDtosList = await _productCategoryService.GetAllAsync();

                if (productCategoriesDtosList == null || !productCategoriesDtosList.Any())
                    return NotFound($"Didnot find any product category");

                return Ok(productCategoriesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("count", Name = "GetCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<long>> GetCount()
        {
            try
            {
                var count = await _productCategoryService.GetCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-paged", Name = "GetPaged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0");

            try
            {
                var productCategoriesDtosList = await _productCategoryService.GetPagedDataAsync(pageNumber, pageSize);

                if (productCategoriesDtosList == null || !productCategoriesDtosList.Any())
                    return NotFound($"Didnot find any product categor");

                return Ok(productCategoriesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddProductCategory")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductCategoryDto>> AddProductCategory(ProductCategoryDto productCategoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewproductCategoryDto = await  _productCategoryService.AddAsync(productCategoryDto,UserId);

                if(NewproductCategoryDto == null) return BadRequest("Cannot add new product Category");

                return CreatedAtRoute("GetById", new { Id = NewproductCategoryDto.Id }, NewproductCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfProductCategories")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> AddRangeOfProductCategories(IEnumerable< ProductCategoryDto> productCategoriesDtos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewproductCategoriesDtosList = await _productCategoryService.AddRangeAsync(productCategoriesDtos, UserId);

                if (NewproductCategoriesDtosList == null || !NewproductCategoriesDtosList.Any()) return BadRequest("Cannot add new product Categories");

                return Ok("Added new product Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateProductCategoryById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateProductCategoryById([FromRoute]long Id, [FromBody] ProductCategoryDto productCategoryDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");
            if(!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
               
                var IsProductCategoryUpdated = await _productCategoryService.UpdateByIdAsync(Id,productCategoryDto);

                if (!IsProductCategoryUpdated) return BadRequest("Cannot Update product Category");

                return Ok("Updated product Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteProductCategoryById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteProductCategoryById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");

            try
            {
                var IsDeleted = await _productCategoryService.DeleteByIdAsync(Id);

                if (!IsDeleted) return BadRequest("Cannot Delete product Category");

                return Ok("Deleted product Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}
