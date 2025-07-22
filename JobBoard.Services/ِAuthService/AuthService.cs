using JobBoard.Domain.Entities;
using JobBoard.Domain.DTO.AuthDto;
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

namespace JobBoard.Services._ِAuthService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration config;

        public AuthService(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , IConfiguration config )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.config = config;
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
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
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
            if (result.Succeeded)
            {
                var roleName = model.user_type.ToString(); // Seeker or Employer
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole(roleName));

                await userManager.AddToRoleAsync(newUser, roleName);
                return new ResultDto(
                    succeeded: true,
                    message: "User registered successfully. Please login and complete your profile."
                    );
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new ResultDto(
                    succeeded: false,
                    message: $"Registration failed: {errors}"
                    );
            }


        }
    }
}
