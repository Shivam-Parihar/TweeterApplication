using Azure.Messaging.ServiceBus;
using Confluent.Kafka;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Services;
using TweetAppApi.Helpers;
using TweetAppApi.Models;


namespace TweetAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRegistrationService _userregrepo;
        private readonly IConfiguration _config;
        public UserController(IUserRegistrationService userregrepo, IConfiguration config)
        {
            _userregrepo = userregrepo;
            _config = config;
        }
        
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModelDto usermodel)
        {
            var result=await _userregrepo.UserRegistration(usermodel);
            //using (var producer =
            //     new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
            //{
            //    try
            //    {
            //        Console.WriteLine(producer.ProduceAsync("tweet_app", new Message<Null, string> { Value = "User Registered Successfully!" })
            //            .GetAwaiter()
            //            .GetResult());
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"Oops, something went wrong: {e}");
            //    }
            //}
            string message="";
            if (result.Success == true)
            {
                message = usermodel.Email + " user registerd" + " on " + DateTime.Now;
            }
            else
            {
                message = result.Message;
            } 
            
            string connectionstr = _config.GetValue<string>("TopicConnectionString");
            var sbClient = new ServiceBusClient(connectionstr);
            var sbSender = sbClient.CreateSender("tweet-app-messaging");
            var sbmessag = new ServiceBusMessage(message);
            await sbSender.SendMessageAsync(sbmessag);
            return Ok(result);


        }
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModelDto usermodel)
        {
            var result = await _userregrepo.UserLogin(usermodel);

            //using (var producer =
            //     new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
            //{
            //    try
            //    {
            //        Console.WriteLine(producer.ProduceAsync("tweet_app", new Message<Null, string> { Value = "User Login Successfully!" })
            //            .GetAwaiter()
            //            .GetResult());
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine($"Oops, something went wrong: {e}");
            //    }
            //}
            string message = "";
            if (result.Success == true)
            {
                message = usermodel.Email + " user login" + " on " + DateTime.Now;
            }
            else
            {
                message = "Username/Password is incorrect";
            }
            //string message = usermodel.Email + " user login" + " on " + DateTime.Now;
            string connectionstr = _config.GetValue<string>("TopicConnectionString");
            var sbClient = new ServiceBusClient(connectionstr);
            var sbSender = sbClient.CreateSender("tweet-app-messaging");
            var sbmessag = new ServiceBusMessage(message);
            await sbSender.SendMessageAsync(sbmessag);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelDto model)
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string emailid = obj.GetEmail(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            model.Email = emailid;
            var result = await _userregrepo.ResetPassword(model);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            GetUserDetailsFromToken obj = new GetUserDetailsFromToken();
            string id = obj.GetUserId(Request.Headers[HeaderNames.Authorization].ToString().Split(" ")[1]);
            var result = await _userregrepo.Logout(id);
            return Ok(result);
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModelDto model)
        {
            var result = await _userregrepo.ForgotPasword(model);
            return Ok(result);
        }


    }
}
