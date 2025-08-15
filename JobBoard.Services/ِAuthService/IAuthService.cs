using Google.Apis.Auth;
using JobBoard.Domain.DTO.AuthDto;
using JobBoard.Domain.DTO.ExternalLoginDto;
using JobBoard.Domain.Entities;
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
        Task<ResultDto> ChangePasswordAsync(ChangePassDto model);
        Task<ResultDto> ResetPasswordAsync(ResetPassDto model);
        Task<ResultDto> ForgotPasswordAsync(ForgetPassDto model);
        Task<ResultDto> SendConfirmationEmailAsync(string email);
        Task<ResultDto> ConfirmEmailAsync(ConfirmEmailDto model);
        //Task<string> RefreshTokenAsync(string token, string refreshToken);
        //Task LogoutAsync();
        //Task<bool> IsEmailConfirmedAsync(string email);
        //Task<bool> ConfirmEmailAsync(string email, string token);


        public Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken);
        public Task<ResultLoginDto> GenerateJwtTokenAsync(ApplicationUser user);
        public Task<ResultLoginDto> ExternalLoginAsync(ExternalLoginReceiverDto model);





    }
}
