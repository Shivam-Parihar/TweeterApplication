using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Response;

namespace TweetApp.Service.Services
{
    public interface ITweetService
    {
        Task<ServiceResponse<IEnumerable<TweetDto>>> GetAllTweets();
        Task<ServiceResponse<IEnumerable<TweetDto>>> GetTweetsByUserId(string userid);
        Task<ServiceResponse<IEnumerable<TweetDto>>> GetTweetsByUserName(string userName,string emailId);
        Task<ServiceResponse<bool>> PostTweet(TweetDto tweetmodel);
        Task<ServiceResponse<bool>> DeleteTweet(string tweetId);
        Task<ServiceResponse<bool>> UpdateTweet(string tweetId,string username, TweetDto tweetmodel);
        Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUsers();
        Task<ServiceResponse<string>> ReplyTweet(string username,string tweetId,string message);
        Task<ServiceResponse<string>> LikeTweet(string username, string tweetId);



    }
}
