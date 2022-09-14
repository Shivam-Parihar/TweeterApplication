using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetApp.Service.ModelDto
{
    public class ForgetPasswordModelDto
    {
        public string Email { get; set; }
        public DateTime DOB { get; set; }
        public string NewPassword { get; set; }
    }
}
