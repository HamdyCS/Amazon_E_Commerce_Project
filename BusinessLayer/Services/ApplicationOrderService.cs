using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using BusinessLayer.Roles;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.UnitOfWork.Contracks;

namespace BusinessLayer.Servicese
{
    public class ApplicationOrderService : IApplicationOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IApplicationService _applicationService;
        private readonly IMailService _mailService;
        private readonly IGenericMapper _genericMapper;
        private readonly IShoppingCartService _shoppingCartService;

        public ApplicationOrderService(IUnitOfWork unitOfWork, IUserService userService, IApplicationService applicationService,
            IApplicationTypeService applicationTypeService, IMailService mailService,
            IGenericMapper genericMapper, IShoppingCartService shoppingCartService)
        {
            this._unitOfWork = unitOfWork;
            this._userService = userService;
            this._applicationService = applicationService;
            this._mailService = mailService;
            this._genericMapper = genericMapper;
            this._shoppingCartService = shoppingCartService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ApplicationOrderDto> AddNewShippedApplicationOrderAsync(long ApplicationId, string DeliveredId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(DeliveredId, nameof(DeliveredId));

            var application = await _applicationService.FindByIdAsync(ApplicationId);
            if (application == null) return null;

            var userDto = await _userService.FindByIdAsync(DeliveredId);
            if (userDto == null) return null;

            var IsThisUserDelivered = await _userService.IsUserInRoleByIdAsync(DeliveredId, new RoleDto { Name = Role.DeliveryAgent });
            if (!IsThisUserDelivered) return null;

            var activeApplictionOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAsync(ApplicationId);
            if (activeApplictionOrder == null || activeApplictionOrder.ApplicationOrderTypeId != (long)EnApplicationOrderType.UnderProcessing || (activeApplictionOrder.Payment.PaymentStatusId != (int)EnPaymentStatus.Succeeded && activeApplictionOrder.Payment.PaymentTypeId != (long)EnPaymentType.CashOnDelivery))
                return null;

            var UserDto = await _userService.FindByIdAsync(activeApplictionOrder.CreatedBy);
            if (UserDto is null) return null;

            var NewApplicationOrder = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrder>(activeApplictionOrder);
            if (NewApplicationOrder is null) return null;


            NewApplicationOrder.CreatedAt = DateTime.UtcNow;
            NewApplicationOrder.ApplicationOrderTypeId = (long)EnApplicationOrderType.Shipped;
            NewApplicationOrder.DeliveryId = DeliveredId;


            await _unitOfWork.applicationOrderRepository.AddAsync(NewApplicationOrder);


            var IsNewApplicationOrderAdded = await _CompleteAsync();
            if (!IsNewApplicationOrderAdded) return null;


            await _mailService.SendEmailAsync(UserDto.Email, subject: $"Your order ({NewApplicationOrder.ApplicationId}) is shipping.", $"Your order ({NewApplicationOrder.ApplicationId}) is shipping.");


            var NewApplicationOrderDto = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrderDto>(NewApplicationOrder);
            return NewApplicationOrderDto;
        }

        public async Task<ApplicationOrderDto> AddNewDeliveredApplicationOrderAsync(long ApplicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            var application = await _applicationService.FindByIdAsync(ApplicationId);
            if (application == null) return null;

            var activeApplictionOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAsync(ApplicationId);
            if (activeApplictionOrder == null || activeApplictionOrder.ApplicationOrderTypeId != (long)EnApplicationOrderType.Shipped)
                return null;

            var UserDto = await _userService.FindByIdAsync(activeApplictionOrder.CreatedBy);
            if (UserDto is null) return null;

            var NewApplicationOrder = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrder>(activeApplictionOrder);
            if (NewApplicationOrder is null) return null;


            NewApplicationOrder.CreatedAt = DateTime.UtcNow;
            NewApplicationOrder.ApplicationOrderTypeId = (long)EnApplicationOrderType.Delivered;


            await _unitOfWork.applicationOrderRepository.AddAsync(NewApplicationOrder);


            var IsNewApplicationOrderAdded = await _CompleteAsync();
            if (!IsNewApplicationOrderAdded) return null;


            await _mailService.SendEmailAsync(UserDto.Email, subject: $"Your order ({NewApplicationOrder.ApplicationId}) is Delivered.", $"Your order ({NewApplicationOrder.ApplicationId}) is Delivered.");


            var NewApplicationOrderDto = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrderDto>(NewApplicationOrder);
            return NewApplicationOrderDto;

        }

        public async Task<ApplicationOrderDto> AddNewUnderProcessingApplicationOrderAsync(long ShoppingCartId, long PaymentId, string UserId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ShoppingCartId, nameof(ShoppingCartId));
            ParamaterException.CheckIfLongIsBiggerThanZero(PaymentId, nameof(PaymentId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));


            var UserDto = await _userService.FindByIdAsync(UserId);
            if (UserDto == null) return null;

            var ShoppingCart = await _shoppingCartService.FindByIdAsync(ShoppingCartId);
            if (ShoppingCart is null) return null;


            var NewApplication = await _applicationService.AddNewOrderApplicationAsync(UserId);
            if (NewApplication is null) return null;

            var NewApplicationOrder = new ApplicationOrder()
            {
                ApplicationId = NewApplication.Id,
                ApplicationOrderTypeId = (long)EnApplicationOrderType.UnderProcessing,
                ShoppingCartId = ShoppingCartId,
                PaymentId = PaymentId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = UserId
            };

            await _unitOfWork.applicationOrderRepository.AddAsync(NewApplicationOrder);

            var IsNewApplicationOrderAdded = await _CompleteAsync();
            if (!IsNewApplicationOrderAdded) return null;


            await _mailService.SendEmailAsync(UserDto.Email, subject: $"Your order ({NewApplication.Id}) is under processing", $"Your order ({NewApplication.Id}) is under processing");

            var NewApplicationOrderDto = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrderDto>(NewApplicationOrder);
            return NewApplicationOrderDto;
        }

        public async Task<ApplicationOrderDto> FindActiveApplicationOrderByApplicationIdAndUserIdAsync(long ApplicationId, string userId)
        {

            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var application = await _applicationService.FindByIdAsync(ApplicationId);
            if (application == null) return null;

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) return null;

            var activeApplictionOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAndUserIdAsync(ApplicationId, userId);
            if (activeApplictionOrder == null)
                return null;


            var activeApplictionOrderDto = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrderDto>(activeApplictionOrder);
            return activeApplictionOrderDto;
        }

        public async Task<ApplicationOrderDto> FindByIdAsync(long ApplicationOrderId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationOrderId, nameof(ApplicationOrderId));


            var applictionOrder = await _unitOfWork.applicationOrderRepository.GetByIdAsNoTrackingAsync(ApplicationOrderId);
            if (applictionOrder == null)
                return null;


            var applictionOrderDto = _genericMapper.MapSingle<ApplicationOrder, ApplicationOrderDto>(applictionOrder);
            return applictionOrderDto;
        }

        public async Task<IEnumerable<ApplicationOrderDto>> GetActiveDeliveredApplicationOrdersAsync()
        {
            var applicationOrdersList = await _unitOfWork.applicationOrderRepository.GetActiveDeliveredApplicationOrdersAsync();
            if (applicationOrdersList == null || !applicationOrdersList.Any())
                return null;


            var applicationOrdersDtosList = _genericMapper.MapCollection<ApplicationOrder, ApplicationOrderDto>(applicationOrdersList);
            return applicationOrdersDtosList;
        }

        public async Task<IEnumerable<ApplicationOrderDto>> GetActiveShippedApplicationOrdersAsync()
        {
            var applicationOrdersList = await _unitOfWork.applicationOrderRepository.GetActiveShippedApplicationOrdersAsync();
            if (applicationOrdersList == null || !applicationOrdersList.Any())
                return null;


            var applicationOrdersDtosList = _genericMapper.MapCollection<ApplicationOrder, ApplicationOrderDto>(applicationOrdersList);
            return applicationOrdersDtosList;
        }

        public async Task<IEnumerable<ApplicationOrderDto>> GetActiveUnderProcessingApplicationOrdersAsync()
        {
            var applicationOrdersList = await _unitOfWork.applicationOrderRepository.GetActiveUnderProcessingApplicationOrdersAsync();
            if (applicationOrdersList == null || !applicationOrdersList.Any())
                return null;


            var applicationOrdersDtosList = _genericMapper.MapCollection<ApplicationOrder, ApplicationOrderDto>(applicationOrdersList);
            return applicationOrdersDtosList;
        }

        public async Task<IEnumerable<ApplicationOrderDto>> TrackApplicationOrderByApplicatonIdAndUserIdAsync(long ApplicationId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var application = await _applicationService.FindByIdAsync(ApplicationId);
            if (application == null) return null;

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) return null;

            var applicationOrdersList = await _unitOfWork.applicationOrderRepository.GetAllApplicationOrdersByApplicatonIdAndUserIdAsync(ApplicationId, userId);
            if (applicationOrdersList == null || !applicationOrdersList.Any())
                return null;

            var applicationOrdersDtosList = _genericMapper.MapCollection<ApplicationOrder, ApplicationOrderDto>(applicationOrdersList);
            return applicationOrdersDtosList;
        }

        public async Task<IEnumerable<ApplicationOrderDto>> TrackApplicationOrderByApplicatonIdAsync(long ApplicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            var application = await _applicationService.FindByIdAsync(ApplicationId);
            if (application == null) return null;


            var applicationOrdersList = await _unitOfWork.applicationOrderRepository.GetAllApplicationOrdersByApplicatonIdAsync(ApplicationId);
            if (applicationOrdersList == null || !applicationOrdersList.Any())
                return null;

            var applicationOrdersDtosList = _genericMapper.MapCollection<ApplicationOrder, ApplicationOrderDto>(applicationOrdersList);
            return applicationOrdersDtosList;
        }
    }
}
