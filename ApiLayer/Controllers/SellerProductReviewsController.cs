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
    [Route("api/seller-product-review")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class SellerProductReviewsController : ControllerBase
    {
        private readonly ISellerProductReviewService _sellerProductReviewService;

        public SellerProductReviewsController(ISellerProductReviewService sellerProductReviewService)
        {
            _sellerProductReviewService = sellerProductReviewService;
        }

        [HttpGet("average-of-stars/seller-product-id/{SellerProductId}", Name = "GetAverageOfStarsBySellerProductIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<double>> GetAverageOfStarsBySellerProductIdAsync(long SellerProductId)
        {
            if (SellerProductId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
               var AvgOfStars = await _sellerProductReviewService.GetAverageOfStarsBySellerProductIdAsync(SellerProductId);

                if (AvgOfStars < 0) return NotFound("Not found any seller product review.");

                return Ok(AvgOfStars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{Id}", Name = "GetSellerProductReviewById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SellerProductReviewDto>> GetById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var sellerProductReview = await _sellerProductReviewService.FindByIdAsync(Id);

                if (sellerProductReview == null) return NotFound($"Not found seller product review. Id = {Id}");

                return Ok(sellerProductReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-seller-product-reviews/seller-product-id/{SellerProductId}", Name = "GetAllSellerProductReviewsBySellerProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SellerProductReviewDto>> GetAllSellerProductReviewsBySellerProductId(long SellerProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(SellerProductId, nameof(SellerProductId));
            try
            {
                var sellerProductReviewsDtosList = await _sellerProductReviewService.GetAllSellerProductReviewsBySellerProductIdAsync(SellerProductId);

                if (sellerProductReviewsDtosList == null || !sellerProductReviewsDtosList.Any())
                    return NotFound($"Didnot find any seller product review.");

                return Ok(sellerProductReviewsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("paged/seller-product-id/{SellerProductId}", Name = "GetPagedSellerProductReviewsBySellerProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<SellerProductReviewDto>>> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize,long SellerProductId)
        {
            if (pageNumber < 1 || pageSize < 1) return BadRequest("pagenumber and pagesize must be bigger than 0.");

            try
            {
                var sellerProductReviewsDtosList = await _sellerProductReviewService.GetPagedSellerProductReviewsBySellerProductIdAsync(pageNumber, pageSize, SellerProductId);

                if (sellerProductReviewsDtosList == null || !sellerProductReviewsDtosList.Any())
                    return NotFound($"Didnot find any seller product review.");

                return Ok(sellerProductReviewsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpPost("", Name = "AddNewSellerProductReview")]
        [Authorize]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<SellerProductReviewDto>> AddNew(SellerProductReviewDto sellerProductReviewDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewSellerProductReviewDto = await _sellerProductReviewService.AddAsync(sellerProductReviewDto, UserId);

                if (NewSellerProductReviewDto == null) return BadRequest("Cannot add new seller product review.");

                return CreatedAtRoute("GetSellerProductReviewById", new { Id = NewSellerProductReviewDto.Id }, NewSellerProductReviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateSellerProductReviewById")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateById([FromRoute] long Id, [FromBody] SellerProductReviewDto sellerProductReviewDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsSellerProductReviewUpdated = await _sellerProductReviewService.UpdateByIdAndUserIdAsync(Id, UserId, sellerProductReviewDto);

                if (!IsSellerProductReviewUpdated) return BadRequest("Cannot Update seller product review.");

                return Ok("Updated seller product review successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("admin/{Id}", Name = "DeleteSellerProductReviewById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var IsSellerProductReviewDeleted = await _sellerProductReviewService.DeleteByIdAsync(Id);

                if (!IsSellerProductReviewDeleted) return BadRequest("Cannot Delete seller product review.");

                return Ok("Deleted seller product review successfully.");
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

                var IsSellerReviewProductDeleted = await _sellerProductReviewService.DeleteByIdAndUserIdAsync(Id, UserId);

                if (!IsSellerReviewProductDeleted) return BadRequest("Cannot Delete seller product review.");

                return Ok("Deleted seller product review successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }

}
