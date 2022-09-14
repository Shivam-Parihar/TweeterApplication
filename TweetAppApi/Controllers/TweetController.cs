using Azure.Messaging.ServiceBus;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Response;
using TweetApp.Service.Services;
using TweetAppApi.Helpers;

namespace TweetAppApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : ControllerBase
    {

        private readonly ITweetService _tweetService;
        private readonly IConfiguration _config;
        public TweetController(ITweetService tweetService,IConfiguration config)
        {
            _tweetService = tweetService;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _tweetService.GetAllUsers();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetAllTweets")]
        public async Task<IActionResult> GetAllTweets()
        {
            var result = await _tweetService.GetAllTweets();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetTweets/{username}")]
        public async Task<IActionResult> GetTweetsByUsername([FromRoute] string username)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string emailId = obj.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var result = await _tweetService.GetTweetsByUserName(username,emailId);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetAllTweetsofUser")]
        public async Task<IActionResult> GetTweetsByUserId()
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string id= obj.GetUserId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);            
            var result = await _tweetService.GetTweetsByUserId(id);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("PostTweet")]
        public async Task<IActionResult> PostTweet(TweetDto tweetmodel)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string id = obj.GetUserId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            string emailid = obj.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            tweetmodel.UserId = id;
            tweetmodel.Email = emailid;
            var result = await _tweetService.PostTweet(tweetmodel);
            return Ok(result);
        }

        [HttpPost]
        [Route("reply/{id}")]
        public async Task<IActionResult> ReplyTweet( [FromRoute]string id,[FromBody]string message)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string username = obj.GetUsername(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var result = await _tweetService.ReplyTweet(username,id,message);
            return Ok(result);
        }

        [HttpPut]
        [Route("like/{id}")]
        public async Task<IActionResult> LikeTweet( [FromRoute] string id)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string username = obj.GetUsername(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var result = await _tweetService.LikeTweet(username,id);
            return Ok(result);
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTweet( [FromRoute] string id, TweetDto tweetmodel)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string username = obj.GetUsername(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var result = await _tweetService.UpdateTweet(id,username,tweetmodel);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTweet( [FromRoute] string id)
        {
            var result = await _tweetService.DeleteTweet(id);
            //using (var producer =
            //     new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
            //{
            //    try
            //    {
            //        Console.WriteLine(producer.ProduceAsync("tweet_app", new Message<Null, string> { Value = "Tweet Deleted!" })
            //            .GetAwaiter()
            //            .GetResult());
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"Oops, something went wrong: {e}");
            //    }
            //}
            string message = " Tweet Deleted ";
            string connectionstr = _config.GetValue<string>("TopicConnectionString");
            var sbClient = new ServiceBusClient(connectionstr);
            var sbSender = sbClient.CreateSender("tweet-app-messaging");
            var sbmessag = new ServiceBusMessage(message);
            await sbSender.SendMessageAsync(sbmessag);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetIdFromToken")]
        public async Task<IActionResult> GetIdFromToken()
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string id = obj.GetUserId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var res = new ServiceResponse<string>()
            {
                Data = id,
                StatusCode = 200,
                Message = "Success",
                Success = true
            };
            return Ok(res);
        }






    }
}
