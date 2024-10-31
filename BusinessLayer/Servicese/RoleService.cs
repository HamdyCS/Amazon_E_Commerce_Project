using BusinessLayer.Contracks;
using BusinessLayer.Dtos;
using BusinessLayer.Exceptions;
using DataAccessLayer.UnitOfWork.Contracks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Servicese
{
    public class RoleService : IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(ILogger<RoleService> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> CheckIfRoleInSystemByNameAsync(string roleName)
        {
           ParamaterException.CheckIfStringIsValid(roleName,nameof(roleName));

            try
            {
                var result =await _unitOfWork.roleManagerRepository.IsRoleExistByName(roleName);

                return result;
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            try
            {
                var roles = await  _unitOfWork.roleManagerRepository.GetAllRolesAsync();

                var RolesDtoList = new List<RoleDto>();

                foreach (var role in roles) RolesDtoList.Add(new RoleDto { Name = role });

                return RolesDtoList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
