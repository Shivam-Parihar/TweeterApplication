
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetAppApi.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }
        public bool IsLoggedIn { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
