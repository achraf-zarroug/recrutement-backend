using backen_dotnet.Data;
using backen_dotnet.Dtos.Account;
using backen_dotnet.Dtos.Offre;
using backen_dotnet.Dtos.Password;
using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using backen_dotnet.Repository;
using backen_dotnet.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
            private readonly UserManager<AppUser> _userManager;
            private readonly ITokenService _tokenService;
            private readonly SignInManager<AppUser> _signInManager;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;

        public AccountController( UserManager<AppUser> userManager,
                                      ITokenService tokenService,
                                      SignInManager<AppUser> signInManager,
                                      IHttpContextAccessor httpContextAccessor,
                                      IUserRepository userRepository,
                                      IMailService mailService)
            {
                _userManager = userManager;
                _tokenService = tokenService;
                _signInManager = signInManager;
                _httpContextAccessor = httpContextAccessor;
                _userRepository = userRepository;
                _mailService = mailService;

            }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("Email not found.");
            }
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Créer le lien de réinitialisation
            var resetLink = $"https://hr.ultimateit.io/resetpassword?token={Uri.EscapeDataString(resetToken)}&email={Uri.EscapeDataString(user.Email)}";
            Console.WriteLine($"Token received: {resetToken}");

            var mailData = new MailData()
            {
                FromEmail = "chrouf.est@gmail.com",
                ToEmails = dto.Email,
                Subject = "Forgot Password",
                Body = $@"
        <h1>Réinitialisation de mot de passe</h1>
        <p>Cliquez sur le lien ci-dessous pour réinitialiser votre mot de passe :</p>
        <a href='{resetLink}'>Réinitialiser le mot de passe</a>
        <p>Si vous n'avez pas demandé de réinitialisation, veuillez ignorer cet email.</p>",
                
            };
            bool result = await _mailService.SendMailAsync(mailData);
            if (result)
            {
                return Ok(new { message = "Email sent successfully." });
            }
            else
            {
                return StatusCode(500, new { message = "Failed to send email." });
            }
            Console.WriteLine($"Reset link: {resetLink}");

            return Ok("A password reset link has been sent to your email.");
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("Invalid request.");
            }
            Console.WriteLine($"Token received: {dto.Token}");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Password reset successfully.");
        }
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("Invalid email.");

            var result = await _userManager.ConfirmEmailAsync(user, dto.Token);
            if (result.Succeeded)
            {
                return Ok(new { message = "Email verified successfully!" });
            }

            return BadRequest("Invalid or expired token.");
        }
        [HttpPost("register")]
       
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest("Email is already in use.");
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    DateCreated = DateTime.Now,
                    PhoneNumber = registerDto.PhoneNumber,
                };
                var createdUser = await _userManager.CreateAsync(appUser,registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
                    var encodedToken = Uri.EscapeDataString(token);
                    var verificationLink =
                               $"https://hr.ultimateit.io/verify-email?token={encodedToken}&email={Uri.EscapeDataString(appUser.Email)}";
                    var mailData = new MailData()
                    {
                        FromEmail = "chrouf.est@gmail.com",
                        ToEmails = appUser.Email,
                        Subject = "Verify your email",
                        Body = $@"
        <h1>Email verification</h1>
        <p>Please click the link to verify your email :</p>
        <a href='{verificationLink}'>Verify your email</a>",

                    };
                    // Envoyer l'email
                   await _mailService.SendMailAsync(mailData);


                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if(roleResult.Succeeded)
                    {

                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = await _tokenService.CreateToken(appUser)
                            });
                    }else{return StatusCode(500, roleResult.Errors);}
                }else{return StatusCode(500, createdUser.Errors);}

            } catch (Exception ex)
            {return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande.");}
        }
        [HttpPost("add-admin")]
        public async Task<IActionResult> AddAdmin([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                    DateCreated = DateTime.Now,
                    PhoneNumber = registerDto.PhoneNumber,

                };
                var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "Admin");
                    if (roleResult.Succeeded)
                    {

                        return Ok(
                            new NewUserDto
                            {
                                UserName = appUser.UserName,
                                Email = appUser.Email,
                                Token = await _tokenService.CreateToken(appUser)
                            });
                    }
                    else { return StatusCode(500, roleResult.Errors); }
                }
                else { return StatusCode(500, createdUser.Errors); }

            }
            catch (Exception ex)
            { return StatusCode(500, "Une erreur s'est produite lors du traitement de la demande."); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginDto.Email.ToLower());
            if(user == null)
            {
                return Unauthorized("Invalid Email Adress!");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);
            if (!result.Succeeded)
            {
                return Unauthorized(" password incorrecte !");
            }
            var token = await _tokenService.CreateToken(user);
            Response.Cookies.Append("auth_token", token, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true 
            });
            

            return Ok(
               token
                );
        }
        
        [HttpGet("user")]
        [Authorize ]
        public async Task<IActionResult> GetUser()
        {
            var appUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            Console.WriteLine("User afficher ",User);
            Console.WriteLine("afficher appUSer",appUser);
            if (appUser == null)
            {
                return NotFound("User not found");
            }
            var roles = await _userManager.GetRolesAsync(appUser);
            //appUser.Roles = roles.ToList();

            return Ok(appUser);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateAppUsereDto updateDto)
        {
            var UserModel = await _userRepository.UpdateAsync(id,updateDto);  
            if (UserModel == null)
            {
                return NotFound();
            }
            return Ok(UserModel);
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var offres = await _userRepository.GetAllAsync();
            return Ok(offres);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var offreModel = await _userRepository.DeleteAsync(id);
            if (offreModel == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
