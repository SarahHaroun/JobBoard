using JobBoard.Domain.DTO.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services._ِAuthService
{
    public interface IAuthService
    {
        Task<ResultDto> RegisterAsync(RegisterDto model);
        Task<ResultLoginDto> LoginAsync(LoginDto model);
        //Task<string> RefreshTokenAsync(string token, string refreshToken);
        //Task LogoutAsync();
        //Task<bool> IsEmailConfirmedAsync(string email);
        //Task<bool> ConfirmEmailAsync(string email, string token);
        //Task<bool> ResetPasswordAsync(ResetPasswordDto model);

    }
}
