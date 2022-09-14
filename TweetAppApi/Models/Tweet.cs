
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TweetAppApi.Models
{
    public class Tweet:TweetBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TweetId { get; set; }
        public string Message { get; set; }

        
    }
}
