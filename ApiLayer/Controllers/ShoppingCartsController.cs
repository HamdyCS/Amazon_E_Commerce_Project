using ApiLayer.Help;
using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Roles;
using BusinessLayer.Servicese;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiLayer.Controllers
{
    [Route("api/shopping-carts")]
    [ApiController]
    [EnableRateLimiting("FixedWindowPolicyByUserIpAddress")]

    public class ShoppingCartsController : ControllerBase
    {
        // عامل ان الفرونت ايند ميبعتش Id
        //shopping cart
        // عاشن ميبعتش بتاع مستخدم اخر بالغلط وتحصل مصيبة
        // فانا من هنا مهندل الموضوع اتوماتيك


        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartsController (IShoppingCartService shoppingCartService)
        {
            this._shoppingCartService = shoppingCartService;
        }


        [HttpGet("", Name = "GetActiveShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ShoppingCartDto>> GetActiveShoppingCart()
        {

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);

                if (ActiveShoppingCartDto == null) return NotFound($"Not found active shopping cart.");

                return Ok(ActiveShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{ShoppingCartId}", Name = "GetShoppingCartById")]
        [Authorize(Roles = Role.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ShoppingCartDto>> GetShoppingCartById(long ShoppingCartId)
        {
            try
            {
                var ShoppingCartDto = await _shoppingCartService.FindByIdAsync(ShoppingCartId);

                if (ShoppingCartDto == null) return NotFound($"Not found shopping cart. Id = {ShoppingCartId}");

                return Ok(ShoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("all", Name = "GetAllUserShoppingCarts")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable< ShoppingCartDto>>> GetAllUserShoppingCarts()
        {

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var UserShoppingCartsDtosList = await _shoppingCartService.GetAllUserShoppingCartsByUserIdAsync(UserId);

                if (UserShoppingCartsDtosList == null || !UserShoppingCartsDtosList.Any()) return NotFound($"Not found ant user shopping cart.");

                return Ok(UserShoppingCartsDtosList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("total-price", Name = "GetTotalPriceOfShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<decimal>> GetTotalPriceOfShoppingCart()
        {

            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
                if (ActiveShoppingCartDto == null) return NotFound($"Not found active shopping cart.");

                var TotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);

                if (TotalPrice < 0) return NotFound("Not found any product in shopping cart");

                return Ok(TotalPrice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("", Name = "AddNewShoppingCart")]
        [Authorize(Roles = Role.Customer)]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> AddNewShoppingCart()
        {
            try
            {
                var UserId = Helper.GetIdFromClaimsPrincipal(User);
                if (UserId == null) return Unauthorized();

                var shoppingCartDto = await _shoppingCartService.AddNewShoppingCart(UserId);

                if (shoppingCartDto == null) return BadRequest("Cannot add new shopping cart.");

                return CreatedAtRoute("GetShoppingCartById",new { ShoppingCartId = shoppingCartDto.Id },shoppingCartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


    }
}