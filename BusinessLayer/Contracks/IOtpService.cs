using BusinessLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracks
{
    public interface IOtpService
    {
        Task<OtpDto> AddNewOtpAsync(OtpDto otpDto);

        Task<OtpDto> GetByIdAsync(long id);

        Task<bool> CheckIfOtpActiveAsync(OtpDto otpDto);

        Task<bool> CheckIfOtpUsedAsync(OtpDto otpDto);

        Task<bool> MakeOtpUsedAsync(OtpDto otpDto);

        Task<bool> CheckIfOtpActiveAndNotUsedAsync(OtpDto otpDto);
    }
}
