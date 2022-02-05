using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlogEngine.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);
        Task<string> SaveImage(IFormFile image);
    }
    public class FileManager : IFileManager
    {
        private string _imagePath;
        public FileManager(IConfiguration config)
        {
            _imagePath = config["Path:Images"];
        }

        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_imagePath, image), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var save_path = Path.Combine(_imagePath);
                if (!Directory.Exists(save_path))
                {
                    Directory.CreateDirectory(save_path);
                }

                var mine = image.FileName.Substring(image.FileName.LastIndexOf('.'));
                var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{mine}";
                using (var fileStream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                var accessor = new HttpContextAccessor();
                var httpContext = accessor.HttpContext;
                var hostValue = string.Empty;
                if (httpContext.Request.IsHttps)
                {
                    hostValue = "https://" + httpContext.Request.Host;
                }
                else
                {
                    hostValue = "http://" + httpContext.Request.Host;
                }
                var p = Path.Combine(save_path, fileName).Replace("wwwroot", hostValue);
                return p;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return "Error";
            }
            
        }
    }
}
