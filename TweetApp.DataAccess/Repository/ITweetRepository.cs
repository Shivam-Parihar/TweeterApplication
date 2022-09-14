using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.Repository
{
    public interface ITweetRepository
    {
        Task<Tweet> GetTweetById(string tweetId);
        Task<IEnumerable<Tweet>> GetAllTweets();
        Task<IEnumerable<Tweet>> GetTweetsByUserId(string id);
        Task<bool> DeleteTweet(string tweetId);
        Task<Tweet> PostTweet(Tweet tweetmodel);
        Task<bool> ReplyToTweet (Tweet tweet);
        Task<bool> LikeToTweet(Tweet tweet);



    }
}
