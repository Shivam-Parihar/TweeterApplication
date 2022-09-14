using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TweetApp.Domain.Models;
using TweetApp.Domain.Repository;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Response;

namespace TweetApp.Service.Services
{
    public class UserRegistrationService:IUserRegistrationService
    {
        private readonly IUserRepository _userrepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserRegistrationService(IUserRepository userrepo, IConfiguration configuration, IMapper mapper)
        {
            _userrepo = userrepo;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<string>> UserLogin(LoginModelDto usermodel)
        {
            try { 
            var userexist = await _userrepo.GetUserByEmail(usermodel.Email);
            if (userexist != null && ( userexist.Password==usermodel.Password))
            {
                var s=_userrepo.UpdateUserStatusLogIn(userexist.UserId);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, userexist.Email),
                    new Claim(ClaimTypes.NameIdentifier, userexist.UserId),
                    new Claim(ClaimTypes.Name, userexist.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                var token = GetToken(authClaims);
                string res = new JwtSecurityTokenHandler().WriteToken(token);
                return new ServiceResponse<string>()
                {
                    Message = "Login successful",
                    StatusCode = 200,
                    Success = true,
                    Data = res
                };

            }
                return new ServiceResponse<string>()
                {
                    Message = "Login Unsuccessful",
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
           

        }

        public async Task<ServiceResponse<string>> UserRegistration(RegisterModelDto usermodel)
        {
            try
            {
                Regex regex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

                bool isValidEmail = regex.IsMatch(usermodel.Email);
                if (!isValidEmail)
                {
                    return new ServiceResponse<string>()
                    {
                        Data = "Unsuccessful",
                        Message = "Email is not valid",
                        StatusCode = 400,
                        Success = false
                    };
                }
                if (usermodel.Email == "" ||usermodel.Username=="" || usermodel.FirstName == "" || usermodel.Gender == "" || usermodel.Password == "")
                {
                    return new ServiceResponse<string>()
                    {
                        Data = "Unsuccessful",
                        Message = "Please Fill all the Mandatory Fields",
                        StatusCode = 400,
                        Success = false

                    };
                }
                var userExists = await _userrepo.GetUserByEmail(usermodel.Email);
                if (userExists != null)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Email already registered..try again with another EmailId",
                        StatusCode = 401,
                        Success = false,
                        Data = null
                    };
                }
                var usernameExists = await _userrepo.GetUserByUsername(usermodel.Username);
                if (usernameExists != null)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Username already taken",
                        StatusCode = 401,
                        Success = false,
                        Data = null
                    };
                }
                var result = await _userrepo.AddUser(_mapper.Map<User>(usermodel));
                if (result!=null)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Registration successful",
                        StatusCode = 200,
                        Success = true,
                        Data = null
                    };

                }
                return new ServiceResponse<string>()
                {
                    Message = "Something went wrong...Registration unsuccessful",
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };

            }
            catch(Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
        }
        public async Task<ServiceResponse<string>> ForgotPasword(ForgetPasswordModelDto usermodel)
        {
            try
            {
                bool status = false;
                var userExists = await _userrepo.GetUserByEmail(usermodel.Email);
                if (userExists.DOB.ToString().Split(" ")[0] == usermodel.DOB.ToString().Split(" ")[0])
                {
                    status = await _userrepo.ForgetPassword(_mapper.Map<ForgetPasswordModel>(usermodel));
                }

                if (status)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Forget Password successful",
                        StatusCode = 200,
                        Success = true,
                        Data = null
                    };
                }

                return new ServiceResponse<string>()
                {
                    Message = "unsuccessful",
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<string>> ResetPassword(ResetPasswordModelDto model)
        {
            try
            {
                bool status = await _userrepo.ResetPassword(_mapper.Map<ResetPasswordModel>(model));
                if (status)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Password Reset successful",
                        StatusCode = 200,
                        Success = true,
                        Data = null
                    };
                }

                return new ServiceResponse<string>()
                {
                    Message = "Password Reset unsuccessful",
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
        }
        public async Task<ServiceResponse<string>> Logout(string id)
        {
            try
            {

                bool status = await _userrepo.UpdateUserStatusLogOut(id);
                if (status)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Logout successful",
                        StatusCode = 200,
                        Success = true,
                        Data = null
                    };
                }

                return new ServiceResponse<string>()
                {
                    Message = "Logout failed",
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
            catch(Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = null
                };
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        
    }
}
