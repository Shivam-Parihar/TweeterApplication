using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TweetApp.Domain.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public IList<Tweet> Tweets { get; set; }
    }
}
