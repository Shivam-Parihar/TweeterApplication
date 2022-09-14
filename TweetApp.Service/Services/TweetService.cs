using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;
using TweetApp.Domain.Repository;
using TweetApp.Service.ModelDto;
using TweetApp.Service.Response;

namespace TweetApp.Service.Services
{
    public class TweetService : ITweetService
    {
        private readonly IUserRepository _userrepo;
        private readonly ITweetRepository _tweetrepo;
        private readonly IMapper _mapper;

        public TweetService(IUserRepository userrepo, IMapper mapper, ITweetRepository tweetrepo)
        {
            _userrepo = userrepo;
            _tweetrepo = tweetrepo;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<IEnumerable<TweetDto>>> GetAllTweets()
        {
            try {

                var res=_mapper.Map<IEnumerable<TweetDto>>(await _tweetrepo.GetAllTweets());
                foreach (var n in res)
                {
                    string id = n.UserId;
                    var user = await _userrepo.GetUserById(id);
                    n.Email = user.Email;
                }
                return new ServiceResponse<IEnumerable<TweetDto>>()
                {
                    Message = "success",
                    StatusCode = 200,
                    Success = true,
                    Data = res
                };
            }
            catch(Exception ex)
            {
                return new ServiceResponse<IEnumerable<TweetDto>>()
                {
                    Message = ex.ToString(),
                    StatusCode = 401,
                    Success = false,
                    Data = new List<TweetDto>()
                };

            }
        }

        public async Task<ServiceResponse<IEnumerable<TweetDto>>> GetTweetsByUserId(string userid)
        {
            try
            {
                var user = await _userrepo.GetUserById(userid);
                var res = _mapper.Map<IEnumerable<TweetDto>>(await _tweetrepo.GetTweetsByUserId(userid));
                foreach(var n in res)
                {
                    n.Email = user.Email;
                }
                return new ServiceResponse<IEnumerable<TweetDto>>()
                {
                    Message = "success",
                    StatusCode = 200,
                    Success = true,
                    Data = res
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<IEnumerable<TweetDto>>()
                {
                    Message = "something went wrong",
                    StatusCode = 401,
                    Success = false,
                    Data = new List<TweetDto>()
                };

            }
        }

        public async Task<ServiceResponse<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {

                var res = _mapper.Map<IEnumerable<UserDto>>(await _userrepo.GetAllUsers());
                
                return new ServiceResponse<IEnumerable<UserDto>>()
                {
                    Message = "success",
                    StatusCode = 200,
                    Success = true,
                    Data = res
                };
            }
            catch (Exception e)
            {
            
                return new ServiceResponse<IEnumerable<UserDto>>()
                {
                    Message = "something went wrong",
                    StatusCode = 401,
                    Success = false,
                    Data = new List<UserDto>()
                };

            }
        }


        public async Task<ServiceResponse<bool>> PostTweet(TweetDto tweetmodel)
        {
            try
            {
                if(tweetmodel==null || tweetmodel.Message==null )
                {
                    return new ServiceResponse<bool>()
                    {
                        Message = "Fill all required fields",
                        StatusCode = 400,
                        Success = false,
                        Data = false
                    };

                }
                tweetmodel.AddedDate = DateTime.Now;
                var res = await _tweetrepo.PostTweet(_mapper.Map<Tweet>(tweetmodel));
                return new ServiceResponse<bool>()
                {
                    Message = "success",
                    StatusCode = 200,
                    Success = true,
                    Data = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>()
                {
                    Message = "something went wrong",
                    StatusCode = 401,
                    Success = false,
                    Data = false
                };

            }
        }

        public async Task<ServiceResponse<string>> ReplyTweet(string username, string tweetId, string message)
        {
            try
            {
                if (tweetId == null || username == null)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Please Fill all the Mandatory Fields",
                        StatusCode = 500,
                        Success = false,
                        Data = null

                    };
                }
                else
                {
                    var userexist = await _userrepo.GetUserByUsername(username);
                    if (userexist == null)
                    {
                        return new ServiceResponse<string>()
                        {
                            Message = "User not found",
                            StatusCode = 500,
                            Success = false,
                            Data = null
                        };
                    }
                    var tweet = await _tweetrepo.GetTweetById(tweetId);
                    if (tweet == null)
                    {
                        return new ServiceResponse<string>()
                        {
                            Message = "Tweet not found",
                            StatusCode = 500,
                            Success = false,
                            Data = null
                        };
                    }
                    Reply tweetReply = new Reply()
                    {
                        ReplyText = message,
                        CreatedOn = DateTime.Now,
                        user = userexist
                    };
                    tweet.TweetReplies.Add(tweetReply);
                    bool isTweetUpdated = await _tweetrepo.ReplyToTweet(tweet);
                        return new ServiceResponse<string>()
                        {
                            Message = isTweetUpdated? "You replied to tweet.":"Something went wrong",
                            StatusCode = isTweetUpdated?200:500,
                            Success = isTweetUpdated,
                            Data = null
                        };
                   
                }
            }
            catch(Exception e)
            {
                return new ServiceResponse<string>
                {
                    Message = e.Message,
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
            
        }

        public async Task<ServiceResponse<string>> LikeTweet(string username, string tweetId)
        {
            try
            {
                if (tweetId == null || username == null)
                {
                    return new ServiceResponse<string>()
                    {
                        Message = "Please Fill all the Mandatory Fields",
                        StatusCode = 500,
                        Success = false,
                        Data = null

                    };
                }
                else
                {
                    var userexist = await _userrepo.GetUserByUsername(username);
                    if (userexist == null)
                    {
                        return new ServiceResponse<string>()
                        {
                            Message = "User not found",
                            StatusCode = 500,
                            Success = false,
                            Data = null
                        };
                    }
                    var tweet = await _tweetrepo.GetTweetById(tweetId);
                    if (tweet == null)
                    {
                        return new ServiceResponse<string>()
                        {
                            Message = "Tweet not found",
                            StatusCode = 500,
                            Success = false,
                            Data = null
                        };
                    }
                    var isAlreadyLiked = tweet.Likes.FirstOrDefault(s => s.UserId == userexist.UserId);
                    if (isAlreadyLiked == null)
                    {
                        tweet.Likes.Add(userexist);
                    }
                    else
                    {
                        tweet.Likes.Remove(isAlreadyLiked);                       
                    }
                    bool isTweetUpdated = await _tweetrepo.LikeToTweet(tweet);
                    if(isTweetUpdated)
                    {
                        return new ServiceResponse<string>()
                        {
                            Message = isAlreadyLiked == null?"User liked the tweet":"User unliked the tweet",
                            StatusCode = 200,
                            Success = true,
                            Data = null
                        };
                    }
                    return new ServiceResponse<string>()
                    {
                        Message = "Something went wrong",
                        StatusCode = 401,
                        Success = false,
                        Data = null
                    };
                }
            }
            catch (Exception e)
            {
                return new ServiceResponse<string>()
                {
                    Message = e.Message,
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<IEnumerable<TweetDto>>> GetTweetsByUserName(string userName,string emailId)
        {
            try
            {
                if (userName == null)
                {
                    return new ServiceResponse<IEnumerable<TweetDto>>()
                    {
                        Message = "Please fill all mandatory fields",
                        StatusCode = 500,
                        Success = false,
                        Data = null
                    };
                }
                else
                {
                    var user = await _userrepo.GetUserByUsername(userName);
                    if (user == null)
                    {
                        return new ServiceResponse<IEnumerable<TweetDto>>()
                        {
                            Message = "User not found",
                            StatusCode = 500,
                            Success = false,
                            Data = null
                        };
                    }
                    else
                    {
                        var userTweets = _mapper.Map<IEnumerable<TweetDto>>(await _tweetrepo.GetTweetsByUserId(user.UserId));
                        foreach (var i in userTweets)
                        {
                            i.Email = emailId;
                        }
                        return new ServiceResponse<IEnumerable<TweetDto>>()
                        {
                            Message = "Success",
                            StatusCode = 500,
                            Success = false,
                            Data = userTweets
                        };
                    }
                }
            }catch(Exception e)
            {
                return new ServiceResponse<IEnumerable<TweetDto>>()
                {
                    Message = "Some Exception occurs",
                    StatusCode = 500,
                    Success = false,
                    Data = null
                };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteTweet(string tweetId)
        {
            try
            {


                var tweetExist = await _tweetrepo.GetTweetById(tweetId);
                if (tweetId == null || tweetExist == null)
                {
                    return new ServiceResponse<bool>()
                    {
                        Message = "Tweet not found",
                        StatusCode = 500,
                        Success = false,
                        Data = false
                    };
                }
                bool res = await _tweetrepo.DeleteTweet(tweetId);
                return new ServiceResponse<bool>()
                {
                    Message = res ? "Tweet Deleted Successfully" : "Something went wrong",
                    StatusCode = res ? 200 : 500,
                    Success = res,
                    Data = res
                };
            }catch(Exception e)
            {
                return new ServiceResponse<bool>()
                {
                    Message = "Some Exception occured",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }


        }

        public async Task<ServiceResponse<bool>> UpdateTweet(string tweetId, string username, TweetDto tweetmodel)
        {
            try
            {
                var tweetExist = await _tweetrepo.GetTweetById(tweetId);
                if (tweetId == null || username == null || tweetExist == null)
                {
                    return new ServiceResponse<bool>()
                    {
                        Message = "Tweet not found",
                        StatusCode = 500,
                        Success = false,
                        Data = false
                    };
                }
                tweetExist.Message = tweetmodel.Message;
                bool result = await _tweetrepo.ReplyToTweet(tweetExist);

                return new ServiceResponse<bool>()
                {
                    Message = result ? "Tweet updated successfully" : "Something went wrong",
                    StatusCode = result ? 200 : 500,
                    Success = result,
                    Data = result
                };
            }catch(Exception e)
            {
                return new ServiceResponse<bool>()
                {
                    Message = "Some Exception occurs",
                    StatusCode = 500,
                    Success = false,
                    Data = false
                };
            }
        }
    }
}
