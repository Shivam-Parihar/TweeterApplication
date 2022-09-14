using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace TweetApp.Domain.Models
{
    public class Tweet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Message { get; set; }

        [BsonRequired]
        public string UserId { get; set; }
        public User user { get; set; }
        public DateTime AddedDate { get; set; }
        public IList<Reply> TweetReplies { get; set; }
        public IList<User> Likes { get; set; }

    }

    

     
}
