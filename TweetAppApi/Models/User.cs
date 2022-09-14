using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetAppApi.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        public string FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Select Gender")]
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "Enter Monile Number"),Phone]
        public string Phone { get; set; }

        [Key]
        [Required(ErrorMessage = "Enter Email"),EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Login Id")]
        public string LoginId { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }
    }
}
