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
    
    public class TweetRepository:ITweetRepository
    {
        
        private readonly IMongoCollection<Tweet> _tweets;
       
        public TweetRepository(IDbClient dbclient)
        {
            _tweets = dbclient.GetTweetCollection();           
        }
        public async Task<IEnumerable<Tweet>> GetAllTweets()
        {
            List<Tweet> allTweets = new List<Tweet>();
            try
            {
                var result = await _tweets.Find(_ => true).ToListAsync();
                return result;
            }catch(Exception e)
            {
                return allTweets;
            }
        }

        public async Task<IEnumerable<Tweet>> GetTweetsByUserId(string id)
        {
            List<Tweet> tweetsByUserId = new List<Tweet>();
            try
            {
                return await _tweets.Find(s => s.UserId == id).ToListAsync();
            }catch(Exception e)
            {
                return tweetsByUserId;
            }
        }       
        public async Task<Tweet> PostTweet(Tweet tweetmodel)
        {
            Tweet postTweet = new Tweet();
            try
            {
                await _tweets.InsertOneAsync(tweetmodel);
                return tweetmodel;
            }catch(Exception e)
            {
                return postTweet;
            }
        }

        public async Task<bool> ReplyToTweet(Tweet tweet)
        {
            try
            {
                var prevtweet = Builders<Tweet>.Filter.Eq(e => e.Id, tweet.Id);
                await _tweets.ReplaceOneAsync(prevtweet, tweet);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public async Task<bool> LikeToTweet(Tweet tweet)
        {
            try
            {
                var prevtweet = Builders<Tweet>.Filter.Eq(e => e.Id, tweet.Id);
                await _tweets.ReplaceOneAsync(prevtweet, tweet);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Tweet> GetTweetById(string tweetId)
        {
            var tweet = await _tweets.Find(t=>t.Id == tweetId).FirstOrDefaultAsync();
            return tweet;
        }

        public async Task<bool> DeleteTweet(string tweetId)
        {
            try
            {
                var  tweetToDelete = Builders<Tweet>.Filter.Eq(e => e.Id, tweetId);
                await _tweets.DeleteOneAsync(tweetToDelete);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
