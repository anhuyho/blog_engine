using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace BlogEngine.DataTransferObject
{
    public class Profile
    {
        [Display(Name ="User Name")]
        public string UserName { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string ImageUrl { get; set; }
    }
}
