using Microsoft.AspNetCore.Http;
using System;
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
    }
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
