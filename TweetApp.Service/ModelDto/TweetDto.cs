

using System;
using System.Collections.Generic;
using TweetApp.Domain.Models;

namespace TweetApp.Service.ModelDto
{
    public class TweetDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime AddedDate { get; set; }        
        public string UserId { get; set; }
        public IList<Reply> TweetReplies { get; set; }
        public IList<UserDto> Likes { get; set; }
    }
}
