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
    [Route("api/product-sub-categories")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class ProductSubCategoriesController : ControllerBase
    {
        private readonly IProductSubCategoryService _productSubCategory;

        public ProductSubCategoriesController(IProductSubCategoryService productSubCategory)
        {
            this._productSubCategory = productSubCategory;
        }

        [HttpGet("{Id}", Name = "GetProductsubCategoryById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetProductsubCategoryById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");

            try
            {
                var productsubCategoryDto = await _productSubCategory.FindByIdAsync(Id);

                if (productsubCategoryDto == null) return NotFound($"Not found product sub category. Id = {Id}");

                return Ok(productsubCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-ar/{NameAr}", Name = "GetProductsubCategoryByNameAr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetProductsubCategoryByNameAr(string NameAr)
        {
            if (string.IsNullOrEmpty(NameAr)) return BadRequest("NameAr is null or empty");

            try
            {
                var productSubCategoryDto = await _productSubCategory.FindByNameArAsync(NameAr);

                if (productSubCategoryDto == null) return NotFound($"Not found product category. NameAR = {NameAr}");

                return Ok(productSubCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-en/{NameEn}", Name = "GetByProductsubCategoryNameEn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetByProductsubCategoryNameEn(string NameEn)
        {
            if (string.IsNullOrEmpty(NameEn)) return BadRequest("NameEn is null or empty");

            try
            {
                var productSubCategoryDto = await _productSubCategory.FindByNameEnAsync(NameEn);

                if (productSubCategoryDto == null) return NotFound($"Not found product sub category. NameEn = {NameEn}");

                return Ok(productSubCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllProductSubCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductCategoryDto>> GetAllProductSubCategories()
        {

            try
            {
                var productSubCategoriesDtosList = await _productSubCategory.GetAllAsync();

                if (productSubCategoriesDtosList == null || !productSubCategoriesDtosList.Any())
                    return NotFound($"Didnot find any product sub category");

                return Ok(productSubCategoriesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("count", Name = "GetCountOfProductSubCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<long>> GetCountOfProductSubCategories()
        {
            try
            {
                var count = await _productSubCategory.GetCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-paged", Name = "GetPagedDataOfProductCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductSubCategoryDto>>> GetPagedDataOfProductCategories([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0");

            try
            {
                var productSubCategoriesDtosList = await _productSubCategory.GetPagedDataAsync(pageNumber, pageSize);

                if (productSubCategoriesDtosList == null || !productSubCategoriesDtosList.Any())
                    return NotFound($"Didnot find any product sub categor");

                return Ok(productSubCategoriesDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddNewProductSubCategory")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductCategoryDto>> AddNewProductSubCategory(ProductSubCategoryDto productSubCategoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewProductSubCategoryDto = await _productSubCategory.AddAsync(productSubCategoryDto, UserId);

                if (NewProductSubCategoryDto == null) return BadRequest("Cannot add new product sub Category");

                return CreatedAtRoute("GetProductsubCategoryById", new { Id = NewProductSubCategoryDto.Id }, NewProductSubCategoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddNewRangeOfProductSubCategories")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> AddNewRangeOfProductSubCategories(IEnumerable<ProductSubCategoryDto> productSubCategoriesDtos)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewproductSubCategoriesDtosList = await _productSubCategory.AddRangeAsync(productSubCategoriesDtos, UserId);

                if (NewproductSubCategoriesDtosList == null || !NewproductSubCategoriesDtosList.Any()) return BadRequest("Cannot add new product sub Categories");

                return Ok("Added new product sub Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateProductSubCategoryById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateProductSubCategoryById([FromRoute] long Id, [FromBody] ProductSubCategoryDto productSubCategoryDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var IsProductSubCategoryUpdated = await _productSubCategory.UpdateByIdAsync(Id, productSubCategoryDto);

                if (!IsProductSubCategoryUpdated) return BadRequest("Cannot Update product sub Category");

                return Ok("Updated product sub Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteProductSubCategoryById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteProductSubCategoryById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");

            try
            {
                var IsDeleted = await _productSubCategory.DeleteByIdAsync(Id);

                if (!IsDeleted) return BadRequest("Cannot Delete product sub Category");

                return Ok("Deleted product sub Categories successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("all/{Id}",Name = "GetAllByProductCategoryIdAsync")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]

        public async Task<ActionResult<IEnumerable<ProductSubCategoryDto>>> GetAllByProductCategoryIdAsync([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero");
            try
            {
                var productSubCategoryDtoList = await _productSubCategory.GetAllByProductCategoryIdAsync(Id);

                if (productSubCategoryDtoList == null || !productSubCategoryDtoList.Any())
                    return NotFound($"Not found any product sub category by product Category Id. Id = {Id}");

                return Ok(productSubCategoryDtoList);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}
