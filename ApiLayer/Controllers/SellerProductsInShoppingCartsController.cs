using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/shopping-carts/{ShoppingCartId}/seller-products")]
    [ApiController]
    [Authorize(Roles = Role.Customer)]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]


    public class SellerProductsInShoppingCartsController : ControllerBase
    {
        private readonly IProductInShoppingCartService _productInShoppingCartService;

        public SellerProductsInShoppingCartsController(IProductInShoppingCartService productInShoppingCartService)
        {
            this._productInShoppingCartService = productInShoppingCartService;
        }

        [HttpGet("{SellerProductInShoppingCartId}", Name = "GetSellerProductInShoppingCartById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SellerProductInShoppingCartDto>> GetSellerProductInShoppingCartById(long ShoppingCartId, long SellerProductInShoppingCartId)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (SellerProductInShoppingCartId < 1) return BadRequest("SellerProductInShoppingCartId must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();


                var sellerProductInShoppingCartDto = await _productInShoppingCartService.
                    FindByIdAndShoppingCartIdAndUserIdAsync(SellerProductInShoppingCartId, ShoppingCartId, UserId);

                if (sellerProductInShoppingCartDto == null) return NotFound($"Not found seller product in shopping cart. Id = {SellerProductInShoppingCartId}");

                return Ok(sellerProductInShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("", Name = "AddNewSellerProductInShoppingCart")]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<SellerProductInShoppingCartDto>> AddNewSellerProductInShoppingCart(long ShoppingCartId, SellerProductInShoppingCartDto productInShoppingCartDto)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (productInShoppingCartDto is null) return BadRequest("SellerProductInShoppingCartDto cannot be null.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewSellerProductInShoppingCartDto = await _productInShoppingCartService.AddAsync(productInShoppingCartDto, ShoppingCartId, UserId);

                if (NewSellerProductInShoppingCartDto == null) return BadRequest("Cannot add new seller product to shopping cart.");

                return CreatedAtRoute("GetSellerProductInShoppingCartById", new { ShoppingCartId = ShoppingCartId, SellerProductInShoppingCartId = NewSellerProductInShoppingCartDto.Id }, NewSellerProductInShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfNewSellerProductInShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<SellerProductInShoppingCartDto>>> AddRangeOfNewSellerProductInShoppingCart(long ShoppingCartId, IEnumerable<SellerProductInShoppingCartDto> ProductsInShoppingCartDtosList)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewSellerProductsInShoppingCartDtosList = await _productInShoppingCartService.AddRangeAsync(ProductsInShoppingCartDtosList, ShoppingCartId, UserId);

                if (NewSellerProductsInShoppingCartDtosList == null || !NewSellerProductsInShoppingCartDtosList.Any()) return BadRequest("Cannot add new seller products to shopping cart.");

                return Ok(NewSellerProductsInShoppingCartDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{SellerProductInShoppingCartId}", Name = "UpdateSellerProductInShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateSellerProductInShoppingCart([FromRoute] long SellerProductInShoppingCartId, long ShoppingCartId, [FromBody] SellerProductInShoppingCartDto productInShoppingCartDto)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (SellerProductInShoppingCartId < 1) return BadRequest("SellerProductInShoppingCartId must be bigger than zero.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsSellerProductInShoppingCartUpdated = await _productInShoppingCartService.UpdateAsync(SellerProductInShoppingCartId, productInShoppingCartDto, ShoppingCartId, UserId);

                if (!IsSellerProductInShoppingCartUpdated) return BadRequest("Cannot Update seller product in shopping cart.");

                return Ok("Updated seller product in shopping cart successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{SellerProductInShoppingCartId}", Name = "DeleteProductFromShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteProductFromShoppingCart([FromRoute] long SellerProductInShoppingCartId, long ShoppingCartId)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");

            if (SellerProductInShoppingCartId < 1) return BadRequest("SellerProductInShoppingCartId must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsSellerProductInShoppingCartDeleted = await _productInShoppingCartService.DeleteAsync(SellerProductInShoppingCartId, ShoppingCartId, UserId);

                if (!IsSellerProductInShoppingCartDeleted) return BadRequest("Cannot Delete seller product in shopping cart.");

                return Ok("Deleted seller product in shopping successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
