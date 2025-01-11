using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Route("api/shopping-carts/{ShoppingCartId}/products")]
    [ApiController]
    public class ProductsInShoppingCartsController : ControllerBase
    {
        private readonly IProductInShoppingCartService _productInShoppingCartService;

        public ProductsInShoppingCartsController(IProductInShoppingCartService productInShoppingCartService)
        {
            this._productInShoppingCartService = productInShoppingCartService;
        }

        [HttpGet("{ProductInShoppingCartId}", Name = "GetProductInShoppingCartById")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductInShoppingCartDto>> GetProductInShoppingCartById(long ShoppingCartId,long ProductInShoppingCartId)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (ProductInShoppingCartId < 1) return BadRequest("ProductInShoppingCartId must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();


                var productInShoppingCartDto = await _productInShoppingCartService.
                    FindByIdAndShoppingCartIdAndUserIdAsync(ProductInShoppingCartId,ShoppingCartId,UserId);

                if (productInShoppingCartDto == null) return NotFound($"Not found product in shopping cart. Id = {ProductInShoppingCartId}");

                return Ok(productInShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("", Name = "AddNewProductInShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ProductInShoppingCartDto>> AddNewProductInShoppingCart(long ShoppingCartId, ProductInShoppingCartDto productInShoppingCartDto)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (productInShoppingCartDto is null) return BadRequest("productInShoppingCartDto cannot be null.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewProductInShoppingCartDto = await _productInShoppingCartService.AddAsync(productInShoppingCartDto,ShoppingCartId,UserId);

                if (NewProductInShoppingCartDto == null) return BadRequest("Cannot add new product to shopping cart.");

                return CreatedAtRoute("GetProductInShoppingCartById",new { ShoppingCartId  = ShoppingCartId, ProductInShoppingCartId = NewProductInShoppingCartDto.Id},NewProductInShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("range", Name = "AddRangeOfNewProductInShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ProductInShoppingCartDto>>> AddRangeOfNewProductInShoppingCart(long ShoppingCartId,IEnumerable< ProductInShoppingCartDto> ProductsInShoppingCartDtosList)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var NewProductsInShoppingCartDtosList = await _productInShoppingCartService.AddRangeAsync(ProductsInShoppingCartDtosList, ShoppingCartId, UserId);

                if (NewProductsInShoppingCartDtosList == null || !NewProductsInShoppingCartDtosList.Any()) return BadRequest("Cannot add new products to shopping cart.");

                return Ok(NewProductsInShoppingCartDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPut("{ProductInShoppingCartId}", Name = "UpdateProductInShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> UpdateProductInShoppingCart([FromRoute] long ProductInShoppingCartId, long ShoppingCartId , [FromBody] ProductInShoppingCartDto productInShoppingCartDto)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");
            if (ProductInShoppingCartId < 1) return BadRequest("ProductInShoppingCartId must be bigger than zero.");

            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsProductInShoppingCartUpdated = await _productInShoppingCartService.UpdateAsync(ProductInShoppingCartId,productInShoppingCartDto,ShoppingCartId,UserId );

                if (!IsProductInShoppingCartUpdated) return BadRequest("Cannot Update product in shopping cart.");

                return Ok("Updated product in shopping cart successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpDelete("{ProductInShoppingCartId}", Name = "DeleteProductInShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<string>> DeleteProductInShoppingCart([FromRoute] long ProductInShoppingCartId, long ShoppingCartId)
        {
            if (ShoppingCartId < 1) return BadRequest("ShoppingCartId must be bigger than zero.");

            if (ProductInShoppingCartId < 1) return BadRequest("Id must be bigger than zero.");

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var IsSellerProductDeleted = await _productInShoppingCartService.DeleteAsync(ProductInShoppingCartId,ShoppingCartId,UserId);

                if (!IsSellerProductDeleted) return BadRequest("Cannot Delete product in shopping cart.");

                return Ok("Deleted product in shopping successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
