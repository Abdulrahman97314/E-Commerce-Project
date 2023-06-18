using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.APIs.Dtos;
using Store.APIs.Errors;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Core.Services;
using System.Security.Claims;
using System.Web;

namespace Store.APIs.Controllers
{
    public class AccountsController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly IEmailSettings emailSettings;
        private readonly IConfiguration configuration;

        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper,IEmailSettings emailSettings,IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.emailSettings = emailSettings;
            this.configuration = configuration;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if(user is null)
                return Unauthorized(new ApiResponse(401, "The email address is incorrect."));
            var password = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!password.Succeeded)
                return Unauthorized(new ApiResponse(401, "The password is incorrect."));
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user,userManager)
            });
        }
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = await userManager.FindByEmailAsync(registerDto.Email);
            if (user is not null)
                return BadRequest(new ApiResponse(400, "This email is already registered"));
            var appUser = new AppUser
            {
                UserName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
                Address = new Address()
            };
            var result = await userManager.CreateAsync(appUser, registerDto.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(appUser, "User");
                return Ok(new UserDto
                {
                    DisplayName = appUser.DisplayName,
                    Email = appUser.Email,
                    Token = await tokenService.CreateTokenAsync(appUser, userManager)
                });
            }
            else return BadRequest(new ApiResponse(400, "Failed to register user"));
        }
        [HttpGet("GetUserData")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserData()
        {
           var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await tokenService.CreateTokenAsync(user, userManager)
            });
        }
        [HttpGet("GetUserAddress")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(u => u.Email == email);
            var addressDto = mapper.Map<AddressDto>(user.Address);
            return addressDto;
        }
        [HttpPut("UpdateUserAddress")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto UpdatedAddressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
            var address = user.Address;

            address.Street = UpdatedAddressDto.Street;
            address.City = UpdatedAddressDto.City;
            address.FirstName = UpdatedAddressDto.FirstName;
            address.LastName = UpdatedAddressDto.LastName;
            address.Country = UpdatedAddressDto.Country;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, "Failed to update user address"));

            var updatedAddressDto = mapper.Map<AddressDto>(address);
            return updatedAddressDto;
        }
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(400, "User not found"));
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var resetPasswordUrl = $"{configuration["FrontEndUrl"]}/resetPassword?email={user.Email}&token={encodedToken}";
            var message = $@"<html>
<head>
    <style>
        /* Define styles for the body */
        body {{
            background-color: #f2f2f2;
            font-family: Arial, sans-serif;
            font-size: 16px;
            margin: 0;
            padding: 0;
        }}

        /* Define styles for the container */
        .container {{
            background-color: #ffffff;
            border-radius: 5px;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
            margin: 20px auto;
            max-width: 600px;
            padding: 20px;
        }}

        /* Define styles for the heading */
        h1 {{
            color: #333;
            font-size: 24px;
            margin-bottom: 16px;
            text-align: center;
        }}

        /* Define styles for the paragraph */
        p {{
            color: #666;
            margin-bottom: 16px;
            text-align: center;
        }}

        /* Define styles for the button container */
        .button-container {{
            text-align: center;
        }}

        /* Define styles for the button */
        .button {{
            background-color: #007bff;
            border-radius: 5px;
            color: #fff;
            display: inline-block;
            font-size: 16px;
            margin: 20px auto;
            padding: 12px 24px;
            text-align: center;
            text-decoration: none;
            transition: background-color 0.3s ease;
        }}

        .button:hover {{
            background-color: #0069d9;
            cursor: pointer;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Reset Password</h1>
        <p>Hello,</p>
        <p>We have sent you this email in response to your request to reset your password on company name.</p>
        <p>To reset your password, please click the button below:</p>
        <div class=""button-container"">
            <a href=""{resetPasswordUrl}"" class=""button"" style=""color: #fff"">Reset Password</a>
        </div>
        <p>Please ignore this email if you did not request a password change.</p>
    </div>
</body>
</html>";

            var email = new Email
            {
                Subject = "Reset Password",
                To = forgotPasswordDto.Email,
                Body = message
            };
            emailSettings.SendEmail(email);

            return Ok(new ApiResponse(200, "The Reset Password link has been sent to your mail")) ;
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest(new ApiResponse(400, "User not found"));
            }
            var result = await userManager.VerifyUserTokenAsync(user, userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetPasswordDto.Token);
            if (!result)
            {
                return BadRequest(new ApiResponse(400, "Invalid password reset token"));
            }
            var resetResult = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!resetResult.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Failed to reset password"));
            }

            return Ok(new ApiResponse(200, "Password has been changed successfully"));
        }
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto )
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
                if (result.Succeeded)
                {
                    return Ok(new ApiResponse(200,"Password has been Changed"));
                }

        }
            return BadRequest(new ApiResponse(500,"Current password is wrong"));
        }
    }
}
