using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.DbSettings
{
    public class DbClient : IDbClient
    {
        private readonly IMongoCollection<Tweet> _tweets;
        private readonly IMongoCollection<User> _users;
        public DbClient(IOptions<TweetAppDatabaseSettings> tweetAppDatabaseSettings)
        {
            var client = new MongoClient(tweetAppDatabaseSettings.Value.ConnectionString);
            var database = client.GetDatabase(tweetAppDatabaseSettings.Value.DatabaseName);
            _tweets = database.GetCollection<Tweet>(tweetAppDatabaseSettings.Value.TweetCollectionName);
            _users = database.GetCollection<User>(tweetAppDatabaseSettings.Value.UserCollectionName);
           
        }
       
        public IMongoCollection<Tweet> GetTweetCollection() => _tweets;

        public IMongoCollection<User> GetUserCollection() => _users;
       

    }
}
