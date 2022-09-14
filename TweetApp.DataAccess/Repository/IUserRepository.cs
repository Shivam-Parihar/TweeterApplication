using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserById(string id);
        Task<User> GetUserByUsername(string username);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> AddUser(User user);       
        Task<bool> IsRegisteredUser(string useremail);
        Task<bool> ResetPassword(ResetPasswordModel model);
        Task<bool> ForgetPassword(ForgetPasswordModel model);
        Task<bool> UpdateUserStatusLogIn(string id);
        Task<bool> UpdateUserStatusLogOut(string id);
    }
}
