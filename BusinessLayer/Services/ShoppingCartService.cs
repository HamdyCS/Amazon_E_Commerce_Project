using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IGenericMapper _genericMapper;
        private readonly ISellerProductService _sellerProductService;

        public ShoppingCartService(IUnitOfWork unitOfWork, IUserService userService,
            IGenericMapper genericMapper, ISellerProductService sellerProductService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
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


            var activeShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(UserId);
            if (activeShoppingCart is not null)
            {
                var activeShoppingCartDto = _genericMapper.MapSingle<ShoppingCart, ShoppingCartDto>(activeShoppingCart);
              
                return activeShoppingCartDto;
            }


            var shoppingCart = new ShoppingCart()
            {
                CreatedAt = DateTime.UtcNow,
                UserId = UserId
            };


            await _unitOfWork.shoppingCartRepository.AddAsync(shoppingCart);
            var IsShoppingCartAdded = await _completeAsync();

            if (!IsShoppingCartAdded) return null;


            var shoppingCartDto = _genericMapper.MapSingle<ShoppingCart, ShoppingCartDto>(shoppingCart);

            return shoppingCartDto;

        }

        public async Task<ShoppingCartDto> FindActiveShoppingCartByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) return null;


            var activeShoppingCart = await _unitOfWork.shoppingCartRepository.GetActiveShoppingCartByUserIdAsync(userId);

            //check if not found active shopping cart create new
            if (activeShoppingCart is null)
            {
               var addNewResult = await AddNewShoppingCart(userId);
               return addNewResult;
            }
            

            var activeShoppingCartDto = _genericMapper.MapSingle<ShoppingCart, ShoppingCartDto>(activeShoppingCart);

            return activeShoppingCartDto;
        }

        public async Task<IEnumerable<ShoppingCartDto>> GetAllUserShoppingCartsByUserIdAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var UserDto = await _userService.FindByIdAsync(userId);
            if (UserDto == null) return null;

            var UserShoppingCartsList = await _unitOfWork.shoppingCartRepository.GetAllUserShoppingCartByUserIdAsync(userId);
            if (UserShoppingCartsList == null || !UserShoppingCartsList.Any()) return [];

            var UserShoppingCartsDtosList = _genericMapper.MapCollection<ShoppingCart, ShoppingCartDto>(UserShoppingCartsList);
           
        
            return UserShoppingCartsDtosList;
        }

        public async Task<decimal> GetTotalPriceInShoppingCartByShoppingCartIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));

            var TotalPrice = await _unitOfWork.shoppingCartRepository.GetTotalPriceInShoppingCartByShoppingCartIdAsync(shoppingCartId);

            return TotalPrice is null ? -1 : (decimal)TotalPrice;

        }

        public async Task<ShoppingCartDto> FindByIdAsync(long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(shoppingCartId, nameof(shoppingCartId));


            var shoppingCart = await _unitOfWork.shoppingCartRepository.GetByIdAsNoTrackingAsync(shoppingCartId);
            if (shoppingCart is null) return null;

            var shoppingCartDto = _genericMapper.MapSingle<ShoppingCart, ShoppingCartDto>(shoppingCart);

            return shoppingCartDto;
        }


    }
}
