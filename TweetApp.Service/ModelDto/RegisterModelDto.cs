using System;
using System.ComponentModel.DataAnnotations;

namespace TweetApp.Service.ModelDto
{
    public class RegisterModelDto
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Username { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }

       
    }
}
