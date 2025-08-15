using Google.Apis.Auth;
using JobBoard.Domain.DTO.AuthDto;
using JobBoard.Domain.Entities;
using JobBoard.Repositories.Persistence;
using JobBoard.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
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
using Google.Apis.Auth;
using JobBoard.Domain.DTO.ExternalLoginDto;

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
            // Check if the user exists and if the password is correct
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return new ResultLoginDto()
                {
                    Succeeded = false,
                    Message = "Invalid email or password."
                };
            }

            // Check if the user has confirmed their email
            if (!user.EmailConfirmed)
            {
                return new ResultLoginDto()
                {
                    Succeeded = false,
                    Message = "Please confirm your email before logging in."
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

            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                EmailConfirmed = false, // Set to false initially, will be confirmed later
                User_Type = model.user_type,

            };
           
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

            // If the user is a Seeker, you can set additional properties here if needed
            if (model.user_type == UserType.Seeker)
            {
                 var seekerProfile = new SeekerProfile 
                 { 
                     UserId = newUser.Id,
                   
                 };
                 await context.SeekerProfiles.AddAsync(seekerProfile);
                 await context.SaveChangesAsync();
            }

            // Send confirmation email directly after register
            try
            {
                await SendConfirmationEmailAsync(newUser.Email);
            }
            catch (Exception ex)
            {
                return new ResultDto(
                    succeeded: false,
                    message: $"Failed to send confirmation email: {ex.Message}"
                    );
            }
            return new ResultDto(
                    succeeded: true,
                    message: "User registered successfully. Please confirm your email."
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
            //var decodedToken = WebUtility.UrlDecode(model.Token);

            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await userManager.ResetPasswordAsync(user, normalToken, model.NewPassword);


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
           

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var clientUrl = config["AppSettings:ClientAppUrl"];

            var resetLink = $"{clientUrl}/reset-password?email={user.Email}&token={encodedToken}";

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


        /*---------------------send Confirm Email Service------------------------*/
        public async Task<ResultDto> SendConfirmationEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResultDto(false, "User not found.");
            }

            if (user.EmailConfirmed)
            {
                return new ResultDto(false, "Email already confirmed.");
            }

            // Generate confirmation token
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            //var encodedToken = WebUtility.UrlEncode(token);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));


            // Confirmation link
            var confirmationLink = $"{config["AppSettings:ClientAppUrl"]}/confirm-email?email={user.Email}&token={encodedToken}";

            // Email body
            var body = $@"
        <p>Hi {user.UserName},</p>
        <p>Please click the button below to confirm your email:</p>
        <div style='text-align:center;'>
            <a href='{confirmationLink}'
               style='background-color:#4CAF50;color:white;padding:10px 20px;
                      text-decoration:none;border-radius:5px;'>
                Confirm Email
            </a>
        </div>
        <p style='margin-top: 30px; font-size: 14px; color: #999;'>
            If you didn't create an account, please ignore this email.
        </p>";

            await emailService.SendEmailAsync(user.Email, "Confirm Your Email", body);

            return new ResultDto(true, "Confirmation email sent.");
        }


        /*------------------------Confirm Email Service--------------------------*/
        public async Task<ResultDto> ConfirmEmailAsync(ConfirmEmailDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new ResultDto(false, "User not found.");

            //var decodedToken = WebUtility.UrlDecode(model.Token);
            var decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            var normalToken = Encoding.UTF8.GetString(decodedBytes);

            var result = await userManager.ConfirmEmailAsync(user, normalToken);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(false, $"Email confirmation failed: {errors}");
            }

            return new ResultDto(true, "Email confirmed successfully.");
        }



        /* -------------------------------External login with google------------------- */

        public async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { config["GoogleAuthSettings:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                throw new Exception("Google token validation failed: " + ex.Message);
            }
        }
        public async Task<ResultLoginDto> GenerateJwtTokenAsync(ApplicationUser user)
        {
            var userRole = await userManager.GetRolesAsync(user);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, userRole.FirstOrDefault() ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: userClaims,
                signingCredentials: signingCredentials
            );

            return new ResultLoginDto
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = DateTime.Now.AddDays(1),
                Role = userRole.FirstOrDefault() ?? ""
            };
        }

        public async Task<ResultLoginDto> ExternalLoginAsync(ExternalLoginReceiverDto model)
        {
            // check google token
            var payload = await VerifyGoogleTokenAsync(model.IdToken);
            if (payload == null)
            {
                return new ResultLoginDto
                {
                    Succeeded = false,
                    Message = "Invalid Google token."
                };
            }

            // 2. search about user
            var user = await userManager.FindByEmailAsync(payload.Email);

            // 3. if user not found --> generate user with role came from client
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return new ResultLoginDto
                    {
                        Succeeded = false,
                        Message = "Failed to create user from Google account."
                    };
                }

                // make sure that the role came from client is already found
                
                if (!Enum.TryParse<UserType>(model.RoleFromClient, true, out var userType))
                {
                    return new ResultLoginDto
                    {
                        Succeeded = false,
                        Message = "Invalid user type."
                    };
                }

                // register the user with the role came from client
                await userManager.AddToRoleAsync(user, userType.ToString());

                // If the user is an Employer, set additional properties
                if (userType == UserType.Employer)
                {
                    var empProfile = new EmployerProfile
                    {
                        CompanyName = model.CompanyName,
                        CompanyLocation = model.CompanyLocation,
                        UserId = user.Id
                    };
                    await context.EmployerProfiles.AddAsync(empProfile);
                }
                else if (userType == UserType.Seeker)
                {
                    var seekerProfile = new SeekerProfile
                    {
                        UserId = user.Id
                    };
                    await context.SeekerProfiles.AddAsync(seekerProfile);
                }

                await context.SaveChangesAsync();

            }



            return await GenerateJwtTokenAsync(user);
        }




    }
}

