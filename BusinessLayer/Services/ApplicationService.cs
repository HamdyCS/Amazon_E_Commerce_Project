using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using BusinessLayer.Mapper.Contracks;
using DataAccessLayer.Entities;
using DataAccessLayer.Enums;
using DataAccessLayer.Identity.Entities;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericMapper _genericMapper;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly IShoppingCartService _shoppingCartService;

        public ApplicationService(IUnitOfWork unitOfWork, IGenericMapper genericMapper, IUserService userService,
            IMailService mailService,IShoppingCartService shoppingCartService)
        {
            this._unitOfWork = unitOfWork;
            this._genericMapper = genericMapper;
            this._userService = userService;
            this._mailService = mailService;
            this._shoppingCartService = shoppingCartService;
        }

        private async Task<bool> _CompleteAsync()
        {
            var NumberOfRowsAfected = await _unitOfWork.CompleteAsync();
            return NumberOfRowsAfected > 0;
        }

        public async Task<ApplicationDto> AddNewOrderApplicationAsync(string userId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) return null;

            var application = new Application()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                ApplicationTypeId = (long)EnApplicationType.Order,

                // For demonstration purposes, we are setting the estimated delivery dates to be between 2 to 5 days from now.
                EstimatedDeliveryFrom = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(2)),
                EstimatedDeliveryTo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5))
            };

            await _unitOfWork.applicationRepository.AddAsync(application);

            var IsApplicationAdded = await _CompleteAsync();
            if (!IsApplicationAdded) return null;

            var applicationDto = _genericMapper.MapSingle<Application, ApplicationDto>(application);
            return applicationDto;

        }

        public async Task<ApplicationDto> FindByIdAsync(long ApplicationId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));

            var application = await _unitOfWork.applicationRepository.GetByIdAsNoTrackingAsync(ApplicationId);
            if (application is null) return null;

            var applicationDto = _genericMapper.MapSingle<Application, ApplicationDto>(application);
            return applicationDto;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllUserApplicationsByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var applicationsList = await _unitOfWork.applicationRepository.GetAllUserApplicationsByUserIdAsync(UserId);
            if (applicationsList is null || !applicationsList.Any()) return null;

            var applicationsDtosList = _genericMapper.MapCollection<Application, ApplicationDto>(applicationsList);
            return applicationsDtosList;
        }

        public async Task<ApplicationDto> AddNewReturnApplicationAsync(string userId, long applicationId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));
            ParamaterException.CheckIfLongIsBiggerThanZero(applicationId, nameof(applicationId));

            var userDto = await _userService.FindByIdAsync(userId);
            if (userDto == null) throw new KeyNotFoundException("User not found.");

            var application = await _unitOfWork.applicationRepository.GetByIdAndUserIdAsync(applicationId,userId);
            if (application is null) throw new KeyNotFoundException("Application not found.");

            //check if the order application is delivered before allowing to create a return application for it
            var lastApplicationOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAndUserIdAsync(applicationId,userId);

            if(lastApplicationOrder == null || lastApplicationOrder.ApplicationOrderTypeId != (long)EnApplicationOrderType.Delivered)
                throw new InvalidOperationException("Only delivered orders can be returned.");

            var activeApplictionOrder = await _unitOfWork.applicationOrderRepository.GetActiveApplicationOrderByApplicationIdAsync(applicationId);
            if (activeApplictionOrder == null || activeApplictionOrder.ApplicationOrderTypeId != (long)EnApplicationOrderType.Delivered)
                return null;

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                
                var NewReturnApplication = new Application()
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    ApplicationTypeId = (long)EnApplicationType.Return
                };

                await _unitOfWork.applicationRepository.AddAsync(NewReturnApplication);

                var IsReturnApplicationAdded = await _CompleteAsync();
                if (!IsReturnApplicationAdded) throw new Exception("ReturnApplication not added.");


                var ReturnApplicationDto = _genericMapper.MapSingle<Application, ApplicationDto>(NewReturnApplication);

                application.ReturnApplicationId = NewReturnApplication.Id;


                await _unitOfWork.applicationRepository.UpdateAsync(applicationId, application);

                var IsOrderApplicationUpdated = await _CompleteAsync();
                if (!IsOrderApplicationUpdated) throw new Exception("OrderApplication not Updated.");


                await _unitOfWork.CommitTransactionAsync();


                await _mailService.SendEmailAsync(userDto.Email, $"Your order ({applicationId}) has been successfully cancelled..", $"Your order ({applicationId}) has been successfully cancelled.");

                return ReturnApplicationDto;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
          
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllReturnApplicationsAsync()
        {
           
            var applicationsList = await _unitOfWork.applicationRepository.GetAllReturnApplicationsAsync();
            if (applicationsList is null || !applicationsList.Any()) return null;

            var applicationsDtosList = _genericMapper.MapCollection<Application, ApplicationDto>(applicationsList);
            return applicationsDtosList;
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllUserReturnApplicationsByUserIdAsync(string UserId)
        {
            ParamaterException.CheckIfStringIsNotNullOrEmpty(UserId, nameof(UserId));

            var userDto = await _userService.FindByIdAsync(UserId);
            if (userDto == null) return null;

            var applicationsList = await _unitOfWork.applicationRepository.GetAllUserReturnApplicationsByUserIdAsync(UserId);
            if (applicationsList is null || !applicationsList.Any()) return null;

            var applicationsDtosList = _genericMapper.MapCollection<Application, ApplicationDto>(applicationsList);
            return applicationsDtosList;
        }

        public async Task<ShoppingCartDto> FindShoppingCartByApplicationIdAndUserIdAsync(long ApplicationId, string userId)
        {
            ParamaterException.CheckIfLongIsBiggerThanZero(ApplicationId, nameof(ApplicationId));
            ParamaterException.CheckIfStringIsNotNullOrEmpty(userId, nameof(userId));

            var shoppingCartid = await _unitOfWork.applicationRepository.GetShoppingCartIdByApplicationIdAndUserIdAsync(ApplicationId, userId);
            if (shoppingCartid == -1) return null;

            var shoppingCartDto = await _shoppingCartService.FindByIdAsync(shoppingCartid);
            return shoppingCartDto;

        }
    }
}
