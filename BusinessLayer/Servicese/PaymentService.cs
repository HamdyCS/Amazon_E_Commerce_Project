using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
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
        private readonly IStripeService _stripeService;
        private readonly IApplicationOrderService _applicationOrderService;

        public PaymentService(IGenericMapper genericMapper, IUserService userService, IUnitOfWork unitOfWork,
            IUserAddressService userAdderssService,
            IShippingCostService shippingCostService, IShoppingCartService shoppingCartService, 
            IApplicationOrderService applicationOrderService,IStripeService stripeService)
        {
            this._genericMapper = genericMapper;
            this._userService = userService;
            this._userAdderssService = userAdderssService;
            this._shippingCostService = shippingCostService;
            this._shoppingCartService = shoppingCartService;
            this._stripeService = stripeService;
            this._applicationOrderService = applicationOrderService;
            this._unitOfWork = unitOfWork;
        }

        private async Task<bool> _CompleteAsync()
        {
            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            return numberOfRowsAffected > 0;
        }
        public async Task<decimal> GetTotalPriceAsync(PaymentDto paymentDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentDto, nameof(paymentDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return -1;


            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto == null || ActiveShoppingCartDto.Id != paymentDto.ShoppingCartId) return -1;

            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentDto.UserAddressId, UserId);
            if (userAddressDto == null) return -1;

            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if (shippingCostDto == null) return -1;


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) return -1;


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;
            return TotalPrice;

        }

        public async Task<bool> PaymentPrePaidAsync(PaymentPrePaidDto paymentPrePaidDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentPrePaidDto, nameof(paymentPrePaidDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var paymentDto = new PaymentDto
            {
                ShoppingCartId = paymentPrePaidDto.ShoppingCartId,
                UserAddressId = paymentPrePaidDto.UserAddressId
            };

            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto == null || ActiveShoppingCartDto.Id != paymentDto.ShoppingCartId) return false;


            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentDto.UserAddressId, UserId);
            if (userAddressDto == null) return false;


            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if (shippingCostDto == null) return false;


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) return false;


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;


            if (!(TotalPrice>0)) return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();


                var payment = _genericMapper.MapSingle<PaymentDto,Payment>(paymentDto);
                if (payment == null) throw new Exception("payment is null");

                payment.TotalPrice = TotalPrice;
                payment.CreatedAt = DateTime.UtcNow;
                payment.PaymentTypeId = (long)EnPaymentType.PrePaid;
                payment.shippingCostId = shippingCostDto.Id;

                await _unitOfWork.paymentRepository.AddAsync(payment);
                var IsPaymentAdded = await _CompleteAsync();

                if (!IsPaymentAdded) throw new Exception("Payment not added");


                var NewApplicationOrderDto = await _applicationOrderService.
                    AddNewUnderProcessingApplicationOrderAsync(ActiveShoppingCartDto.Id, payment.Id, UserId);

                if (NewApplicationOrderDto == null) throw new Exception("not new application order added");


                var newStripeToken = await _stripeService.CreateStripeTokenAsync(paymentPrePaidDto.CardInfo);
                if (newStripeToken == null) throw new Exception("New strip token is null");


                var IsStripeChargeCompletedSuccessfuly = await _stripeService.CreateStripeChargeAsync(paymentPrePaidDto.CardInfo,
                    newStripeToken.Id, (long)TotalPrice * 100, "EGP", "Test payment");

                if(!IsStripeChargeCompletedSuccessfuly) throw new Exception("not stripe charge completed sucessfuly");


                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<bool> PaymentCashOnDeliveryAsync(PaymentDto paymentDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentDto, nameof(paymentDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

           

            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto == null || ActiveShoppingCartDto.Id != paymentDto.ShoppingCartId) return false;


            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentDto.UserAddressId, UserId);
            if (userAddressDto == null) return false;


            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if (shippingCostDto == null) return false;


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) return false;


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;


            if (!(TotalPrice > 0)) return false;

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var payment = _genericMapper.MapSingle<PaymentDto, Payment>(paymentDto);
                if (payment == null) throw new Exception("payment is null");

                payment.TotalPrice = TotalPrice;
                payment.CreatedAt = DateTime.UtcNow;
                payment.PaymentTypeId = (long)EnPaymentType.CashOnDelivery;
                payment.shippingCostId = shippingCostDto.Id;

                await _unitOfWork.paymentRepository.AddAsync(payment);
                var IsPaymentAdded = await _CompleteAsync();

                if (!IsPaymentAdded) throw new Exception("Payment not added");


                var NewApplicationOrderDto = await _applicationOrderService.
                    AddNewUnderProcessingApplicationOrderAsync(ActiveShoppingCartDto.Id, payment.Id, UserId);

                if (NewApplicationOrderDto == null) throw new Exception("not new application order added");

               
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }
    }
}
