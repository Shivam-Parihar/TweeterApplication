using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetApp.Domain.Models;

namespace TweetApp.Domain.DbSettings
{
    public class TweetAppDatabaseSettings  
    {
        public string ConnectionString { get; set; } = null;
        public string DatabaseName { get; set; } = null;
        public string TweetCollectionName { get; set; } = null;
        public string UserCollectionName { get; set; } = null;
        public string CommentCollectionName { get; set; } = null;


    }
}
