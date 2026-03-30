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
        private readonly ISellerProductInShoppingCartService _productInShoppingCartService;

        public SellerProductsInShoppingCartsController(ISellerProductInShoppingCartService productInShoppingCartService)
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



            var UserId = Helper.GetIdFromClaimsPrincipal(User);
            if (UserId == null) return Unauthorized();


            var sellerProductInShoppingCartDto = await _productInShoppingCartService.
                FindByIdAndShoppingCartIdAndUserIdAsync(SellerProductInShoppingCartId, ShoppingCartId, UserId);

            if (sellerProductInShoppingCartDto == null) return NotFound($"Not found seller product in shopping cart. Id = {SellerProductInShoppingCartId}");

            return Ok(sellerProductInShoppingCartDto);

        }


        [HttpPost("", Name = "AddNewSellerProductInShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ShoppingCartDto>> AddNewSellerProductInShoppingCart(long ShoppingCartId, AddSellerProductToShoppingCartDto productInShoppingCartDto)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (productInShoppingCartDto is null) return BadRequest("SellerProductInShoppingCartDto cannot be null.");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var UserId = Helper.GetIdFromClaimsPrincipal(User);
            if (UserId == null) return Unauthorized();

            var shoppingCartDto = await _productInShoppingCartService.AddAsync(productInShoppingCartDto, ShoppingCartId, UserId);

            if (shoppingCartDto == null) return BadRequest(error: "Cannot add new seller product to shopping cart.");

            return Ok(shoppingCartDto);

        }


        [HttpPost("bulk", Name = "AddRangeOfNewSellerProductInShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ShoppingCartDto>> AddRangeOfNewSellerProductInShoppingCart(long ShoppingCartId, IEnumerable<AddSellerProductToShoppingCartDto> ProductsInShoppingCartDtosList)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var UserId = Helper.GetIdFromClaimsPrincipal(User);
            if (UserId == null) return Unauthorized();

            var shoppingCartDto = await _productInShoppingCartService.AddRangeAsync(ProductsInShoppingCartDtosList, ShoppingCartId, UserId);

            if (shoppingCartDto == null) return BadRequest("Cannot add new seller products to shopping cart.");

            return Ok(shoppingCartDto);

        }


        [HttpPut("{SellerProductInShoppingCartId}", Name = "UpdateSellerProductInShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ShoppingCartDto>> UpdateSellerProductInShoppingCart([FromRoute] long SellerProductInShoppingCartId, [FromBody] AddSellerProductToShoppingCartDto productInShoppingCartDto)
        {
            if (SellerProductInShoppingCartId < 1) return BadRequest("SellerProductInShoppingCartId must be bigger than zero.");



            var UserId = Helper.GetIdFromClaimsPrincipal(User);
            if (UserId == null) return Unauthorized();

            var shoppingCartDto = await _productInShoppingCartService.UpdateAsync(SellerProductInShoppingCartId, productInShoppingCartDto, UserId);

            if (shoppingCartDto == null) return BadRequest("Cannot Update seller product in shopping cart.");

            return Ok(shoppingCartDto);

        }


        [HttpDelete("{id}", Name = "DeleteProductFromShoppingCart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteProductFromShoppingCart([FromRoute]long ShoppingCartId, [FromRoute] long id)
        {


            var UserId = Helper.GetIdFromClaimsPrincipal(User);
            if (UserId == null) return Unauthorized();

            //create deleteProductFromShoppingCartDto 
            var deleteProductFromShoppingCartDto = new DeleteProductFromShoppingCartDto
            {
                SellerProductInShoppingCartId = id,
                ShoppingCartId = ShoppingCartId
            };
            var shoppingCartDto = await _productInShoppingCartService.DeleteAsync(deleteProductFromShoppingCartDto, UserId);

            if (shoppingCartDto == null) return BadRequest("Cannot Delete seller product in shopping cart.");

            return Ok(shoppingCartDto);

        }
    }
}
