using JobBoard.Domain.DTO.AuthDto;
using JobBoard.Domain.Entities;
using JobBoard.Repositories.Persistence;
using JobBoard.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Services._ِAuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;
        private readonly IEmailService emailService;

        public AuthService(UserManager<ApplicationUser> userManager ,
            RoleManager<IdentityRole> roleManager , 
            IConfiguration config , ApplicationDbContext context, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
            this.context = context;
            this.emailService = emailService;
        }

        /*------------------------Login Service--------------------------*/
        public async Task<ResultLoginDto> LoginAsync(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return new ResultLoginDto()
                {
                    Succeeded = false,
                    Message = "Invalid email or password."
                };
            }
            var userRole = await userManager.GetRolesAsync(user);  // Get the roles of the user (list of roles)

            List<Claim> userClaims = new List<Claim>();
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            userClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // Unique identifier for the token
            userClaims.Add(new Claim(ClaimTypes.Role, userRole.FirstOrDefault()?? ""));


            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"])); // Secret key for signing the token
            var expiration = model.RememberMe? DateTime.Now.AddDays(7) : DateTime.Now.AddDays(1); // Token expiration time
            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);  // Signing credentials for the token
                // Create the JWT token
            var token = new JwtSecurityToken(                   
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: expiration,
                claims: userClaims,
                signingCredentials: signingCredentials
            );
            return new ResultLoginDto()
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,   //token.ValidTo,
                Role = userRole.FirstOrDefault() ?? ""
            };


        }

        /*------------------------Register Service--------------------------*/
        public async Task<ResultDto> RegisterAsync(RegisterDto model)
        {
          var user= await userManager.FindByEmailAsync(model.Email);   
            if(user != null)
                return new ResultDto(
                    succeeded : false,
                    message : "User already exists."
                    );
            if (!Enum.IsDefined(typeof(UserType), model.user_type))
                return new ResultDto(
                    succeeded: false,
                    message: "Invalid user type."
                    );
            ApplicationUser newUser = new ApplicationUser();
            newUser.UserName = model.UserName;
            newUser.Email = model.Email;
            newUser.User_Type = model.user_type;


            var result = await userManager.CreateAsync(newUser, model.Password);

            // Check if the user creation was successful
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Registration failed: {errors}"
                    );
            }
                        
             var roleName = model.user_type.ToString(); // Seeker or Employer
               if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));

                await userManager.AddToRoleAsync(newUser, roleName);

            // If the user is an Employer, set additional properties
            if (model.user_type == UserType.Employer)
            {
                var empProfile= new EmployerProfile
                {
                    CompanyName = model.CompanyName,
                    CompanyLocation = model.CompanyLocation,
                    UserId = newUser.Id
                };
               await context.EmployerProfiles.AddAsync(empProfile);
               await context.SaveChangesAsync();
            }
            return new ResultDto(
                    succeeded: true,
                    message: "User registered successfully. Please login and complete your profile."
                    );
            }


        /*------------------------Change Password Service--------------------------*/
        public async Task<ResultDto> ChangePasswordAsync(ChangePassDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }
           
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Password change failed: {errors}"
                    );
            }
            return new ResultDto(
                succeeded: true,
                message: "Password changed successfully."
                );

        }



        /*----------------------Reset Password Service-----------------------*/
        public async Task<ResultDto> ResetPasswordAsync(ResetPassDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }
            var decodedToken = WebUtility.UrlDecode(model.Token);


            var result = await userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);


            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Password reset failed: {errors}"
                    );
            }

            return new ResultDto(
                succeeded:true,
                message: "success");
        }


        /*----------------------Forget Password Service-----------------------*/
        public async Task<ResultDto> ForgotPasswordAsync(ForgetPassDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto(
                    succeeded: false,
                    message: "User not found."
                    );
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebUtility.UrlEncode(token);
            var encodedEmail = WebUtility.UrlEncode(model.Email);
            var clientUrl = config["AppSettings:ClientAppUrl"];

            var resetLink = $"{clientUrl}/reset-password?email={encodedEmail}&token={encodedToken}";

            var subject = "Reset Your Password";

            var body = $@"
                 <p>Hi {user.UserName},</p>
                 <p>We received a request to reset your password. Click the button below to choose a new password:</p>

               <div style='text-align:center; margin-top:20px;'>
                    <a href='{resetLink}' 
                   style='background-color:#4CAF50; color:white; padding:10px 20px;
                  text-decoration:none; border-radius:5px; display:inline-block;'>
                       Reset Password
                   </a>
              </div>

             <p style='margin-top: 30px; font-size: 14px; color: #999;'>
                    If you didn't request this, please ignore this email.
             </p>";


            await emailService.SendEmailAsync(model.Email, subject, body);
            return new ResultDto(
                succeeded: true,
                message: "If the email exists in our system, a reset link has been sent"
                );
        }


    }
}

