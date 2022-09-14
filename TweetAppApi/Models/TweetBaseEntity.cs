using System;


namespace TweetAppApi.Models
{
    public class TweetBaseEntity
    {
       
        public int AddedBy { get; set; }
        public int? EditBy { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
