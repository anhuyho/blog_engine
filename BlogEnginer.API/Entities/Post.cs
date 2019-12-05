using System;
using System.ComponentModel.DataAnnotations;

namespace BlogEnginer.API.Entites
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
    }
}
