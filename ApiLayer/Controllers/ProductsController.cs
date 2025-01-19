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
    [Route("api/product")]
    [ApiController]
    [Authorize(Roles = Role.Admin)]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }

        [HttpGet("{Id}", Name = "GetProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var productDto = await _productService.FindByIdAsync(Id);

                if (productDto == null) return NotFound($"Not found product. Id = {Id}");

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-ar/{NameAr}", Name = "GetProductByNameAr")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetByNameAr(string NameAr)
        {
            if (string.IsNullOrEmpty(NameAr)) return BadRequest("NameAr is null or empty.");

            try
            {
                var productDto = await _productService.FindByNameArAsync(NameAr);

                if (productDto == null) return NotFound($"Not found product. NameAR = {NameAr}");

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("name-en/{NameEn}", Name = "GetProductByNameEn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetByNameEn(string NameEn)
        {
            if (string.IsNullOrEmpty(NameEn)) return BadRequest("NameEn is null or empty.");

            try
            {
                var productDto = await _productService.FindByNameEnAsync(NameEn);

                if (productDto == null) return NotFound($"Not found product. NameEn = {NameEn}");

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetAll()
        {

            try
            {
                var productsDtosList = await _productService.GetAllAsync();

                if (productsDtosList == null || !productsDtosList.Any())
                    return NotFound($"Didnot find any product.");

                return Ok(productsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpGet("all-order-by-best-seler-desc", Name = "GetAllOrderByBestSellerDesc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDto>> GetAllOrderByBestSellerDesc()
        {

            try
            {
                var productsDtosList = await _productService.GetAllOrderByBestSellerDescAsync();

                if (productsDtosList == null || !productsDtosList.Any())
                    return NotFound($"Didnot find any product.");

                return Ok(productsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("count", Name = "GetCountOfProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<long>> GetCount()
        {
            try
            {
                var count = await _productService.GetCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-paged", Name = "GetPagedDataOfProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0.");

            try
            {
                var productsDtosList = await _productService.GetPagedDataAsync(pageNumber, pageSize);

                if (productsDtosList == null || !productsDtosList.Any())
                    return NotFound($"Didnot find any product.");

                return Ok(productsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("all-paged-order-by-best-seler-desc", Name = "GetPagedOrderByBestSellerDesc")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ProductCategoryDto>>> GetPagedOrderByBestSellerDesc([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0.");

            try
            {
                var productsDtosList = await _productService.GetPagedOrderByBestSellerDescAsync(pageNumber, pageSize);

                if (productsDtosList == null || !productsDtosList.Any())
                    return NotFound($"Didnot find any product.");

                return Ok(productsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddNewProduct")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductDto>> AddNew(ProductDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewproductDto = await _productService.AddAsync(productDto, UserId);

                if (NewproductDto == null) return BadRequest("Cannot add new product.");

                return CreatedAtRoute("GetProductById", new { Id = NewproductDto.Id }, NewproductDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfProducts")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<string>> AddRange(IEnumerable<ProductDto> productsDtosList)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewproductsDtosList = await _productService.AddRangeAsync(productsDtosList, UserId);

                if (NewproductsDtosList == null || !NewproductsDtosList.Any()) return BadRequest("Cannot add new product Categories.");

                return Ok("Added new products successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateProductById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateById([FromRoute] long Id, [FromBody] ProductDto productDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {

                var IsProductUpdated = await _productService.UpdateByIdAsync(Id, productDto);

                if (!IsProductUpdated) return BadRequest("Cannot Update product.");

                return Ok("Updated product successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteProductById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var IsDeleted = await _productService.DeleteByIdAsync(Id);

                if (!IsDeleted) return BadRequest("Cannot Delete product.");

                return Ok("Deleted product successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("search/{name}",Name = "SearchByName")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ProductSearchResultDto>>> SearchByName(string name, [FromQuery] int pageSize)
        {
            if (pageSize < 1) return BadRequest("pageSize must be bigger than 0");

            if (string.IsNullOrEmpty(name)) return Ok();


            var lang = Request.Headers["lang"].ToString();
            if (lang == null) return BadRequest("Cannot find lang var in header");


            try
            {
                if(lang.ToLower()=="ar")
                {
                    var productSearchResultsDtos = await _productService.SearchByNameArAsync(name, pageSize);
                    return Ok(productSearchResultsDtos);
                }
                else if(lang.ToLower()=="en")
                {
                    var productSearchResultsDtos = await _productService.SearchByNameEnAsync(name, pageSize);
                    return Ok(productSearchResultsDtos);
                }
                else
                {
                    return BadRequest("This language cannot be accepted.");
                }
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

    }

}
