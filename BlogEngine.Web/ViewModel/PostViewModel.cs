﻿using Microsoft.AspNetCore.Http;

namespace BlogEngine.ViewModel
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public IFormFile Image { get; set; }
    }
}
