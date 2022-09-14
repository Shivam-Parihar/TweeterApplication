using MongoDB.Driver;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.DbSettings
{
    public interface IDbClient
    {
        IMongoCollection<Tweet> GetTweetCollection();
        IMongoCollection<User> GetUserCollection();
       
    }
}
