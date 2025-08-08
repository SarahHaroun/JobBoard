using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.AuthDto;
using JobBoard.Repositories.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;

namespace JobBoard.Services._ِAuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;
        private readonly ApplicationDbContext context;

        public AuthService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IConfiguration config )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
            this.context = context;
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
            var signingCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);  // Signing credentials for the token
                // Create the JWT token
            var token = new JwtSecurityToken(                   
                issuer: config["JWT:ValidIssuer"],
                audience: config["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: userClaims,
                signingCredentials: signingCredentials
            );
            return new ResultLoginDto()
            {
                Succeeded = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = DateTime.Now.AddDays(1),   //token.ValidTo,
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

        public async Task<ResultLoginDto> ExternalLoginAsync(string idToken, string roleFromClient)
        {
            // check google token
            var payload = await VerifyGoogleTokenAsync(idToken);
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
                if (!await roleManager.RoleExistsAsync(roleFromClient))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleFromClient));
                }

                // register the user with the role came from client
                await userManager.AddToRoleAsync(user, roleFromClient);
            }



            return await GenerateJwtTokenAsync(user);
        }








    }
}

