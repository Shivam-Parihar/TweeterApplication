using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Response;

namespace TweetApp.Service.Services
{
    public interface IUserRegistrationService
    {
        Task<ServiceResponse<string>> UserRegistration(RegisterModelDto usermodel);
        Task<ServiceResponse<string>> UserLogin(LoginModelDto usermodel);
        Task<ServiceResponse<string>> ForgotPasword(ForgetPasswordModelDto usermodel);
        Task<ServiceResponse<string>> ResetPassword(ResetPasswordModelDto model);
        Task<ServiceResponse<string>> Logout(string id);


    }
}
