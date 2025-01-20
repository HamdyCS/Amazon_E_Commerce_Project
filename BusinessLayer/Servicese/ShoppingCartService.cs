using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IProductInShoppingCartService _productInShoppingCartService;
        private readonly IGenericMapper _genericMapper;
        private readonly ISellerProductService _sellerProductService;

        public ShoppingCartService(IUnitOfWork unitOfWork, IUserService userService,IProductInShoppingCartService productInShoppingCartService,
            IGenericMapper genericMapper, ISellerProductService sellerProductService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._productInShoppingCartService = productInShoppingCartService;
            this._genericMapper = genericMapper;
            this._sellerProductService = sellerProductService;
        }

        private async Task<bool> _completeAsync()
        {
            var numbersOfRowsAffeted = await _unitOfWork.CompleteAsync();
            return numbersOfRowsAffeted > 0;
        }

        public async Task<ShoppingCartDto> AddNewShoppingCart(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) return null;


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCart is not null) return null;

            var shoppingCart = new ShoppingCart()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = UserId
            };


            await _unitOfWork.shoppingCartRepository.AddAsync(shoppingCart);
            var IsShoppingCartAdded = await _completeAsync();

            if (!IsShoppingCartAdded) return null;


            var shoppingCartDto = _genericMapper.MapSingle<ShoppingCart,ShoppingCartDto>(shoppingCart);

            return shoppingCartDto;

        }

        public async Task<ShoppingCartDto> FindActiveShoppingCartByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var UserDto = await _userService.FindByIdAsync(userId);
            if (UserDto == null) return null;


            var ActiveShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(userId);
            if (ActiveShoppingCart is null) return null;

            var ActiveShoppingCartDto = _genericMapper.MapSingle<ShoppingCart,ShoppingCartDto>(ActiveShoppingCart);

            var productsinShoppingCartDtosList = await _productInShoppingCartService.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(ActiveShoppingCart.Id);

            if (productsinShoppingCartDtosList is null)
                return ActiveShoppingCartDto;

            ActiveShoppingCartDto.ProductsInShoppingCartsDtosList = productsinShoppingCartDtosList.ToList();

            return ActiveShoppingCartDto;
        }

        public async Task<IEnumerable<ShoppingCartDto>> GetAllUserShoppingCartsByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var UserDto = await _userService.FindByIdAsync(userId);
            if (UserDto == null) return null;

            var UserShoppingCartsList = await _unitOfWork.shoppingCartRepository.GetAllUserShoppingCartByUserIdAsync(userId);
            if (UserShoppingCartsList == null || !UserShoppingCartsList.Any()) return null;

            var UserShoppingCartsDtosList = _genericMapper.MapCollection<ShoppingCart,ShoppingCartDto>(UserShoppingCartsList);
            if (UserShoppingCartsDtosList == null || !UserShoppingCartsDtosList.Any()) return null;


            foreach (var userShoppingCartDto in UserShoppingCartsDtosList)
            {
                var productsinShoppingCartDtosList = await _productInShoppingCartService.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(userShoppingCartDto.Id);

                if (productsinShoppingCartDtosList is null)
                    continue;

                userShoppingCartDto.ProductsInShoppingCartsDtosList = productsinShoppingCartDtosList.ToList();
            }

            return UserShoppingCartsDtosList;
        }

        public async Task<decimal> GetTotalPriceInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));

            var TotalPrice = await _unitOfWork.shoppingCartRepository.GetTotalPriceInShoppingCartByShoppingCartIdAsync(shoppingCartId);

            return TotalPrice is null? -1 : (decimal)TotalPrice;

        }

        public async Task<ShoppingCartDto> FindByIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));
           

            var shoopingCart = await _unitOfWork.shoppingCartRepository.GetByIdAsNoTrackingAsync(shoppingCartId);
            if (shoopingCart is null) return null;

            var ShoppingCartDto = _genericMapper.MapSingle<ShoppingCart, ShoppingCartDto>(shoopingCart);

            var productsinShoppingCartDtosList = await _productInShoppingCartService.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(shoopingCart.Id);

            if (productsinShoppingCartDtosList is null)
                return ShoppingCartDto;

            ShoppingCartDto.ProductsInShoppingCartsDtosList = productsinShoppingCartDtosList.ToList();

            return ShoppingCartDto;
        }


    }
}
