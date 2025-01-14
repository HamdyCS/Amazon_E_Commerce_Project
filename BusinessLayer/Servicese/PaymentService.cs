using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.UnitOfWork.Contracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class PaymentService : IPaymentService
    {
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAddressService _userAdderssService;
        private readonly IShippingCostService _shippingCostService;
        private readonly IShoppingCartService _shoppingCartService;

        public PaymentService(IGenericMapper genericMapper,IUserService userService,IUnitOfWork unitOfWork,
            IUserAddressService userAdderssService,
            IShippingCostService shippingCostService, IShoppingCartService shoppingCartService)
        {
            this._genericMapper = genericMapper;
            this._userService = userService;
            this._userAdderssService = userAdderssService;
            this._shippingCostService = shippingCostService;
            this._shoppingCartService = shoppingCartService;
            this._unitOfWork = unitOfWork;
        }

        private async Task<bool> CompleteAsync()
        {
            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            return numberOfRowsAffected > 0;
        }
        public async Task<decimal> GetTotalPriceAsync(PaymentDto paymentDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentDto, nameof(paymentDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return  -1;


            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if(ActiveShoppingCartDto == null|| ActiveShoppingCartDto.Id != paymentDto.ShoppingCartId) return -1;

            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentDto.UserAddressId, UserId);
            if(userAddressDto == null) return -1;

            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if(shippingCostDto == null) return -1;


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) return -1;


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;
            return TotalPrice;

        }


    }
}
