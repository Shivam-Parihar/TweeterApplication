using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace TweetApp.Domain.Models
{
    public class Reply
    {
        public string ReplyText { get; set; }
        public DateTime CreatedOn { get; set; }
        public User user { get; set; }      

    }
}
