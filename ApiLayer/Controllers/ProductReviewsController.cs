using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/products/{productId}/reviews")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ProductReviewsController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;

        public ProductReviewsController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }

        [HttpGet("avg", Name = "GetAverageOfStarsByProductIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<double>> GetAverageOfStarsByProductIdAsync(long productId)
        {
            if (productId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
               var AvgOfStars = await _productReviewService.GetAverageOfStarsByProductIdAsync(productId);

                if (AvgOfStars < 0) return NotFound("Not found any product review.");

                return Ok(AvgOfStars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{Id}", Name = "GetProductReviewById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductReviewDto>> GetById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var productReview = await _productReviewService.FindByIdAsync(Id);

                if (productReview == null) return NotFound($"Not found product review. Id = {Id}");

                return Ok(productReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllProductReviewsByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductReviewDto>> GetAllProductReviewsBySellerProductId(long productId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(productId, nameof(productId));
            try
            {
                var productReviewsDtosList = await _productReviewService.GetAllProductReviewsByProductIdAsync(productId);

                if (productReviewsDtosList == null || !productReviewsDtosList.Any())
                    return NotFound($"Didnot find any product review.");

                return Ok(productReviewsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-paged", Name = "GetPagedProductReviewsBySellerProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<ProductReviewDto>>> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize,long productId)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0.");

            try
            {
                var productReviewsDtosList = await _productReviewService.GetPagedProductReviewsByProductIdAsync(pageNumber, pageSize, productId);

                if (productReviewsDtosList == null || !productReviewsDtosList.Any())
                    return NotFound($"Didnot find any product review.");

                return Ok(productReviewsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddNewProductReview")]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductReviewDto>> AddNew(ProductReviewDto productReviewDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewProductReviewDto = await _productReviewService.AddAsync(productReviewDto, UserId);

                if (NewProductReviewDto == null) return BadRequest("Cannot add new product review.");

                return CreatedAtRoute("GetProductReviewById", new { Id = NewProductReviewDto.Id }, NewProductReviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateProductReviewById")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateById([FromRoute] long Id, [FromBody] ProductReviewDto productReviewDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsProductReviewUpdated = await _productReviewService.UpdateByIdAndUserIdAsync(Id, UserId, productReviewDto);

                if (!IsProductReviewUpdated) return BadRequest("Cannot Update product review.");

                return Ok("Updated product review successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}/admin", Name = "DeleteProductReviewById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var IsProductReviewDeleted = await _productReviewService.DeleteByIdAsync(Id);

                if (!IsProductReviewDeleted) return BadRequest("Cannot Delete product review.");

                return Ok("Deleted product review successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteSellerProductReviewByIdAndUserId")]   
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteByIdAndUserId([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsProductReviewDeleted = await _productReviewService.DeleteByIdAndUserIdAsync(Id, UserId);

                if (!IsProductReviewDeleted) return BadRequest("Cannot Delete product review.");

                return Ok("Deleted product review successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }

}
