using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.DbSettings;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.Repository
{
    
    public class UserRepository :IUserRepository
    {
        
        private readonly IMongoCollection<User> _users;
        public UserRepository(IDbClient dbclient)
        {
            _users = dbclient.GetUserCollection();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User userByEmail = new User();
            try
            {
                var user = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch(Exception e)
            {
                return userByEmail;
            }
            
        }
        public async Task<User> GetUserById(string id)
        {
            User userById = new User();
            try
            {
                var user = await _users.Find(u => u.UserId == id).FirstOrDefaultAsync();
                return user;
            }catch(Exception e)
            {
                return userById;
            }
        }

        public async Task<User> GetUserByUsername(string username)
        {
            User userByUSername = new User();
            try
            {
                var user = await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
                return user;
            }catch(Exception e)
            {
                return userByUSername;
            }
        }

        public async Task<User> AddUser(User user)
        {
            User userAdded = new User();
            try
            {
                await _users.InsertOneAsync(user);
                return user;
            }catch(Exception e)
            {
                return userAdded;
            }
        }       

        public async Task<bool> ForgetPassword(ForgetPasswordModel model)
        {
            try
            {
                var user = await _users.Find(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (user.DOB.ToString().Split(" ")[0] == model.DOB.ToString().Split(" ")[0])
                {
                    var updateDef = Builders<User>.Update.Set(u => u.Password, model.NewPassword);
                    await _users.UpdateOneAsync(o => o.Email == model.Email, updateDef);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            List<User> allUsers = new List<User>();
            try
            {
                return await _users.Find(user => true).ToListAsync();
            }catch(Exception e)
            {
                return allUsers;
            }

        }

        public async Task<bool> IsRegisteredUser(string useremail)
        {
            try
            {
                var user = await _users.Find(u => u.Email == useremail).FirstOrDefaultAsync();
                if (user != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        

        public async Task<bool> ResetPassword(ResetPasswordModel model)
        {
            try
            {
                var user = await _users.Find(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (model.OldPassword == user.Password)
                {
                    var updateDef = Builders<User>.Update.Set(u => u.Password, model.NewPassword);
                    await _users.UpdateOneAsync(o => o.Email == model.Email, updateDef);
                    return true;
                }
            }catch(Exception e)
            {
                return false; 
            }
            return false;
        }

       

        public async Task<bool> UpdateUserStatusLogIn(string id)
        {
            try
            {
                var updateDef = Builders<User>.Update.Set(u => u.IsActive, true);
                await _users.UpdateOneAsync(u => u.UserId == id, updateDef);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> UpdateUserStatusLogOut(string id)
        {
            try
            {
                var updateDef = Builders<User>.Update.Set(u => u.IsActive, false);
                await _users.UpdateOneAsync(u => u.UserId == id, updateDef);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
