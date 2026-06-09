using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;

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
        private readonly IApplicationService _applicationService;
        private readonly IApplicationOrderService _applicationOrderService;
        private readonly ISellerProductService _sellerProductService;

        public PaymentService(IGenericMapper genericMapper, IUserService userService, IUnitOfWork unitOfWork,
            IUserAddressService userAdderssService,
            IShippingCostService shippingCostService, IShoppingCartService shoppingCartService,
            IApplicationOrderService applicationOrderService, IStripeService stripeService, IApplicationService applicationService
            , ISellerProductService sellerProductService)
        {
            this._genericMapper = genericMapper;
            this._userService = userService;
            this._userAdderssService = userAdderssService;
            this._shippingCostService = shippingCostService;
            this._shoppingCartService = shoppingCartService;
            this._stripeService = stripeService;
            this._applicationService = applicationService;
            this._applicationOrderService = applicationOrderService;
            this._unitOfWork = unitOfWork;
            this._sellerProductService = sellerProductService;
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


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) return -1;


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;
            return TotalPrice;

        }

        public async Task<PrePaidResultDto> PaymentPrePaidAsync(PaymentPrePaidDto paymentPrePaidDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentPrePaidDto, nameof(paymentPrePaidDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var paymentDto = new PaymentDto
            {
                ShoppingCartId = paymentPrePaidDto.ShoppingCartId,
                UserAddressId = paymentPrePaidDto.UserAddressId
            };

            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto == null || ActiveShoppingCartDto.Id != paymentDto.ShoppingCartId)
                throw new KeyNotFoundException($"Active shopping cart not found.");



            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentDto.UserAddressId, UserId);
            if (userAddressDto == null) throw new KeyNotFoundException($"User address not found. Id: {paymentDto.UserAddressId}");
            ;


            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if (shippingCostDto == null) throw new KeyNotFoundException($"Shipping cost not found for city Id: {userAddressDto.CityId}");


            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) throw new KeyNotFoundException($"Shopping cart total price not found. Id: {ActiveShoppingCartDto.Id}");

            var sellerProductsInCart = await _unitOfWork.SellerProductsInShoppingCartRepository.GetAllSellerProductsInShoppingCartByShoppingCartIdAsync(ActiveShoppingCartDto.Id);
            if (sellerProductsInCart == null || !sellerProductsInCart.Any())
                throw new KeyNotFoundException($"Not found any seller products in cart. ShoppingCartId: {ActiveShoppingCartDto.Id}"); ;

            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;




            try
            {
                await _unitOfWork.BeginTransactionAsync();

                //get payment from db by ShoppingCartId and UserId to check if there is any pending payment for this shopping cart and user
                var payment = await _unitOfWork.paymentRepository.GetByShoppingCartIdAndUserIdAsync(paymentDto.ShoppingCartId, UserId);

                //if null create new payment
                if (payment == null)
                {
                    payment = _genericMapper.MapSingle<PaymentDto, Payment>(paymentDto);
                    if (payment == null) throw new Exception("payment is null");

                    payment.TotalPrice = TotalPrice;
                    payment.CreatedAt = DateTime.UtcNow;
                    payment.PaymentTypeId = (long)EnPaymentType.PrePaid;
                    payment.shippingCostId = shippingCostDto.Id;
                    payment.PaymentStatusId = (int)EnPaymentStatus.Pending;

                    await _unitOfWork.paymentRepository.AddAsync(payment);
                    var IsPaymentAdded = await _CompleteAsync();

                    if (!IsPaymentAdded) throw new Exception("Payment not added");
                }

              

                /*
                var newStripeToken = await _stripeService.CreateStripeTokenAsync(paymentPrePaidDto.CardInfo);
                if (newStripeToken == null) throw new Exception("New strip token is null");


                var IsStripeChargeCompletedSuccessfuly = await _stripeService.CreateStripeChargeAsync(paymentPrePaidDto.CardInfo,
                    newStripeToken.Id, (long)TotalPrice * 100, "EGP", "Test payment");

                if (!IsStripeChargeCompletedSuccessfuly) throw new Exception("not stripe charge completed sucessfuly");
                */

                //stripe session

                //new payment dto to create session with it
                var NewPaymentDto = new PaymentDto
                {
                    Id = payment.Id,
                    ShoppingCartId = ActiveShoppingCartDto.Id,
                    UserAddressId = paymentDto.UserAddressId,
                };

                //create session dto
                var createSessionDto = new CreateSessionDto
                {
                    paymentDto = NewPaymentDto,
                    PaymentId = payment.Id,
                    TotalPrice = TotalPrice,
                    ShippingCost = shippingCostDto.Price,
                    SuccessUrl = paymentPrePaidDto.SuccessUrl,
                    CancelUrl = paymentPrePaidDto.CancelUrl
                };

                //create stripe session
                var stripeDto = await _stripeService.CreateSessionAsync(createSessionDto);
                if (stripeDto == null) throw new Exception("stripe dto is null");

                payment.SessionId = stripeDto.SessionId;
                _unitOfWork.paymentRepository.Update(payment);

                var IsPaymentUpdated = await _CompleteAsync();
                if (!IsPaymentUpdated) throw new Exception("Failed to update payment");

                await _unitOfWork.CommitTransactionAsync();

                var result = new PrePaidResultDto
                {
                    SessionUrl = stripeDto.SessionUrl,
                    SessionId = stripeDto.SessionId
                };

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<CashOnDeliveryResultDto> PaymentCashOnDeliveryAsync(PaymentCashOnDeliveryDto paymentCashOnDeliveryDto, string UserId)
        {
            ParamaterException.CheckIfObjectIfNotNull(paymentCashOnDeliveryDto, nameof(paymentCashOnDeliveryDto));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));



            var ActiveShoppingCartDto = await _shoppingCartService.FindActiveShoppingCartByUserIdAsync(UserId);
            if (ActiveShoppingCartDto == null || ActiveShoppingCartDto.Id != paymentCashOnDeliveryDto.ShoppingCartId)
                throw new KeyNotFoundException($"ActiveShoppingCart not found.");

            var userAddressDto = await _userAdderssService.FindByIdAndUserIdAsync(paymentCashOnDeliveryDto.UserAddressId, UserId);
            if (userAddressDto == null) throw new KeyNotFoundException($"UserAddress not found. Id = {paymentCashOnDeliveryDto.UserAddressId}");


            var shippingCostDto = await _shippingCostService.FindByCityIdAsync(userAddressDto.CityId);
            if (shippingCostDto == null) throw new KeyNotFoundException($"Shipping cost not found for city id = {userAddressDto.CityId}");

            var ShoppingCartTotalPrice = await _shoppingCartService.GetTotalPriceInShoppingCartAsync(ActiveShoppingCartDto.Id);
            if (ShoppingCartTotalPrice == -1) throw new KeyNotFoundException($"Shopping cart total price not found for cart id = {ActiveShoppingCartDto.Id}");


            var TotalPrice = ShoppingCartTotalPrice + shippingCostDto.Price;


            try
            {
                await _unitOfWork.BeginTransactionAsync();

                //get payment from db by ShoppingCartId and UserId to check if there is any pending payment for this shopping cart and user
                var payment = await _unitOfWork.paymentRepository.GetByShoppingCartIdAndUserIdAsync(paymentCashOnDeliveryDto.ShoppingCartId, UserId);

                //if payment is not null update UserAddressId and shippingCostId if there is any change in them to make sure that the payment is up to date with the latest user address and shipping cost

                if (payment != null)
                {
                    if (payment.UserAddressId != paymentCashOnDeliveryDto.UserAddressId)
                    {
                        payment.UserAddressId = paymentCashOnDeliveryDto.UserAddressId;
                    }

                    if (payment.shippingCostId != shippingCostDto.Id)
                    {
                        payment.shippingCostId = shippingCostDto.Id;
                    }

                    payment.TotalPrice = TotalPrice;

                    _unitOfWork.paymentRepository.Update(payment);

                    var IsPaymentUpdated = await _CompleteAsync();
                    if (!IsPaymentUpdated) throw new InvalidOperationException("Payment not updated");
                }

                //if null create new payment
                if (payment == null)
                {

                    payment = _genericMapper.MapSingle<PaymentCashOnDeliveryDto, Payment>(paymentCashOnDeliveryDto);
                    if (payment == null) throw new Exception("payment is null");

                    payment.TotalPrice = TotalPrice;
                    payment.CreatedAt = DateTime.UtcNow;
                    payment.PaymentTypeId = (long)EnPaymentType.CashOnDelivery;
                    payment.shippingCostId = shippingCostDto.Id;
                    payment.PaymentStatusId = (int)EnPaymentStatus.Pending;

                    await _unitOfWork.paymentRepository.AddAsync(payment);
                    var IsPaymentAdded = await _CompleteAsync();

                    if (!IsPaymentAdded) throw new InvalidOperationException("Payment not added");
                }


                //add new application
                var NewApplication = await _applicationService.AddNewOrderApplicationAsync(UserId);
                if (NewApplication is null) throw new InvalidOperationException("New application not added");

                var NewApplicationOrderDto = await _applicationOrderService.
                    AddNewUnderProcessingApplicationOrderAsync(ActiveShoppingCartDto.Id, payment.Id, UserId, NewApplication.Id);
                if (NewApplicationOrderDto == null) throw new InvalidOperationException("not new application order added");

                //deactive shopping cart
                await _shoppingCartService.DeactiveShoppingCartAsync(ActiveShoppingCartDto.Id);



                //update stock quantity for each product in shopping cart
                Dictionary<long, int> sellerProductsIdAndQuantit = ActiveShoppingCartDto.SellerProducts.
                    ToDictionary(sp => sp.SellerProductId, sp => sp.Quantity);




                var isStocksUpdated = await _sellerProductService.UpdateSellerProductsStockAsync(sellerProductsIdAndQuantit, EnOperation.Subtract);
                if(!isStocksUpdated) throw new InvalidOperationException("Failed to update stocks for products in shopping cart");


                await _unitOfWork.CommitTransactionAsync();
                return new CashOnDeliveryResultDto { ApplicationId = NewApplication.Id };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return null;
            }
        }

        public async Task<bool> UpdatePaymentStatusByIdAsync(long paymentId, EnPaymentStatus enPaymentStatus)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentId, nameof(paymentId));

            try
            {
                //get payment by id
                var payment = await _unitOfWork.paymentRepository.GetByIdAsNoTrackingAsync(paymentId);
                if (payment == null) return false;

                //update payment status
                payment.PaymentStatusId = (int)enPaymentStatus;
                await _unitOfWork.paymentRepository.UpdateAsync(payment.Id, payment);

                var IsPaymentUpdated = await _CompleteAsync();
                return IsPaymentUpdated;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentByIdAndInvoiceIdAsync(long paymentId, string invoiceId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentId, nameof(paymentId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(invoiceId, nameof(invoiceId));

            try
            {
                //get payment by id
                var payment = await _unitOfWork.paymentRepository.GetByIdAndInvoiceIdAsync(paymentId, invoiceId);
                if (payment == null) return false;

                //update payment status
                payment.InvoiceId = invoiceId;
                await _unitOfWork.paymentRepository.UpdateAsync(payment.Id, payment);

                var IsPaymentUpdated = await _CompleteAsync();
                return IsPaymentUpdated;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdatePaymentStatusAndInvoiceIdByIdAsync(long paymentId, EnPaymentStatus enPaymentStatus, string invoiceId, long shoppingCartId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(paymentId, nameof(paymentId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(invoiceId, nameof(invoiceId));


            try
            {
                await _unitOfWork.BeginTransactionAsync();
                //get payment by id
                var payment = await _unitOfWork.paymentRepository.GetByIdAsNoTrackingAsync(paymentId);
                if (payment == null) return false;

                //update payment
                payment.PaymentStatusId = (int)enPaymentStatus;

                payment.InvoiceId = invoiceId;
                 _unitOfWork.paymentRepository.Update(payment);

                var IsPaymentUpdated = await _CompleteAsync();

                if (!IsPaymentUpdated) throw new InvalidOperationException("Payment not Updated");

                //deactive shopping cart if payment is succeeded
                if (enPaymentStatus == EnPaymentStatus.Succeeded)
                {

                    var shoppingCart = await _shoppingCartService.FindByIdAsync(shoppingCartId);
                    if(shoppingCart == null) throw new InvalidOperationException("Shopping cart not found");

                    await _shoppingCartService.DeactiveShoppingCartAsync(shoppingCartId);

                    //add new application
                    var NewApplication = await _applicationService.AddNewOrderApplicationAsync(shoppingCart.UserId);
                    if (NewApplication is null) throw new InvalidOperationException("Failed to add new application");


                    var NewApplicationOrderDto = await _applicationOrderService.
                        AddNewUnderProcessingApplicationOrderAsync(shoppingCart.Id, payment.Id, shoppingCart.UserId, NewApplication.Id);

                    if (NewApplicationOrderDto == null) throw new InvalidOperationException("Failed to add new application order");

                    //update stock quantity for each product in shopping cart
                    Dictionary<long, int> sellerProductsIdAndQuantit =  shoppingCart.SellerProducts.ToDictionary(sp => sp.SellerProductId, sp => sp.Quantity);

                    var isStocksUpdated = await _sellerProductService.UpdateSellerProductsStockAsync(sellerProductsIdAndQuantit, EnOperation.Subtract);
                    if (!isStocksUpdated) throw new InvalidOperationException("Failed to update stocks for products in shopping cart");
                }

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }
        }

        public async Task<PaymentDto> GetPaymentByApplicationOrderIdAndUserIdAsync(long applicationOrderId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(applicationOrderId, nameof(applicationOrderId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var payment = await _unitOfWork.paymentRepository.FindByApplicationOrderIdAndUserIdAsync(applicationOrderId, userId);

            if (payment == null) throw new KeyNotFoundException("Payment not found");

            var paymentDto = _genericMapper.MapSingle<Payment, PaymentDto>(payment);
            return paymentDto;
        }

        public async Task<PaymentDto> GetBySessionIdAndUserIdAsync(string sessionId, string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(sessionId, nameof(sessionId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var payment = await _unitOfWork.paymentRepository.GetBySessionIdAndUserIdAsync(sessionId, userId);
            if (payment == null) throw new KeyNotFoundException("Payment not found");

            var paymentDto = _genericMapper.MapSingle<Payment, PaymentDto>(payment);
            return paymentDto;

        }
    }
}
