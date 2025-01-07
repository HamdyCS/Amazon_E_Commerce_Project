using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ApiLayer.Controllers
{
    [Route("api/seller-products")]
    [ApiController]
    public class SellerProductsController : ControllerBase
    {
        private readonly ISellerProductService _SellerProductService;

        public SellerProductsController(ISellerProductService sellerProductService)
        {
            _SellerProductService = sellerProductService;
        }

        [HttpGet("{Id}", Name = "GetSellerProductById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SellerProductDto>> GetById(long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var sellerProduct = await _SellerProductService.FindByIdAsync(Id);

                if (sellerProduct == null) return NotFound($"Not found seller product. Id = {Id}");

                return Ok(sellerProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
       
        [HttpGet("all-seller-product/product-id/{ProductId}", Name = "GetAllSellerProductsByProductId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SellerProductDto>> GetAllSellerProductsByProductId(long ProductId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ProductId,nameof(ProductId));
            try
            {
                var sellerProductsDtosList = await _SellerProductService.GetAllByProductIdOrderByPriceAscAsync(ProductId);

                if (sellerProductsDtosList == null || !sellerProductsDtosList.Any())
                    return NotFound($"Didnot find any seller product.");

                return Ok(sellerProductsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all-seller-product-of-seller", Name = "GetAllSellerProductsOfSeller")]
        [Authorize(Roles = Role.Seller)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SellerProductDto>>> GetAllSellerProductsOfSeller()
        {

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var sellerProductsDtosList = await _SellerProductService.GetAllSellerProductsBySellerIdAsync(UserId);

                if (sellerProductsDtosList == null || !sellerProductsDtosList.Any())
                    return NotFound($"Didnot find any seller product.");

                return Ok(sellerProductsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("admin/all-seller-products-of-seller/{UserId}", Name = "GetAllSellerProductsOfSellerBySellerId")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<SellerProductDto>>> GetAllSellerProductsOfSellerBySellerId(string UserId)
        {

            try
            {
              
                var sellerProductsDtosList = await _SellerProductService.GetAllSellerProductsBySellerIdAsync(UserId);

                if (sellerProductsDtosList == null || !sellerProductsDtosList.Any())
                    return NotFound($"Didnot find any seller product.");

                return Ok(sellerProductsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        

        [HttpPost("", Name = "AddNewSellerProduct")]
        [Authorize(Roles = Role.Seller)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<SellerProductDto>> AddNew(SellerProductDto sellerProductDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewSellerProductDto = await _SellerProductService.AddAsync(sellerProductDto, UserId);

                if (NewSellerProductDto == null) return BadRequest("Cannot add new seller product.");

                return CreatedAtRoute("GetSellerProductById", new { Id = NewSellerProductDto.Id }, NewSellerProductDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfSellerProducts")]
        [Authorize(Roles = Role.Seller)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<SellerProductDto>>> AddRange(IEnumerable<SellerProductDto> sellerProductsDtosList)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewSellerProductsDtosList = await _SellerProductService.AddRangeAsync(sellerProductsDtosList, UserId);

                if (NewSellerProductsDtosList == null || !NewSellerProductsDtosList.Any()) return BadRequest("Cannot add new product Categories.");

                return Ok(NewSellerProductsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{Id}", Name = "UpdateSellerProductById")]
        [Authorize(Roles = Role.Seller)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateById([FromRoute] long Id, [FromBody] SellerProductDto sellerProductDto)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");


            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsSellerProductUpdated = await _SellerProductService.UpdateByIdAndUserIdAsync(Id,UserId,sellerProductDto);

                if (!IsSellerProductUpdated) return BadRequest("Cannot Update seller product.");

                return Ok("Updated seller product successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("admin/{Id}", Name = "DeleteSellerProductById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteById([FromRoute] long Id)
        {
            if (Id < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var IsSellerProductDeleted = await _SellerProductService.DeleteByIdAsync(Id);

                if (!IsSellerProductDeleted) return BadRequest("Cannot Delete seller product.");

                return Ok("Deleted product successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{Id}", Name = "DeleteSellerProductByIdAndUserId")]
        [Authorize(Roles = Role.Seller)]
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

                var IsSellerProductDeleted = await _SellerProductService.DeleteByIdAndUserIdAsync(Id,UserId);

                if (!IsSellerProductDeleted) return BadRequest("Cannot Delete seller product.");

                return Ok("Deleted product successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }

}
