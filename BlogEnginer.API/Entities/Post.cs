using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogEngine.API.Entities
{
    public abstract class BaseEntity
    {
        [Required]
        public int Id { get; set; }
    }
    public class Post: BaseEntity
    {

        [Display(Name = "Post Title")]
        public string PostName { get; set; }

        [Display(Name = "Post Description")]
        public string PostDescription { get; set; }

        [Display(Name = "Post Content")]
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public Post()
        {
            TimeStamp = DateTime.Now;
        }
        public User User { get; set; }
    }
    public class User: BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string IdentityId { get; set; }
        public string SiteName { get; set; }
        public List<Post> Posts { get; set; } 
    }
}
