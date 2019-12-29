using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogEngine.DataTransferObject
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string PostName { get; set; }

        public string PostDescription { get; set; }

        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }
        public PostViewModel()
        {
            TimeStamp = DateTime.Now;
        }
        public int UserId { get; set; }
    }
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string SiteName { get; set; }
        public string IdentityId { get; set; }
        
    }
}
