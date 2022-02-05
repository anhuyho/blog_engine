using Microsoft.Extensions.Configuration;

namespace BlogEngine.DataTransferObject
{
    public static class Contanst
    {
        //public static string ApiEndPoint = "https://localhost:1122";
        //public static string WebEndPoint = "http://localhost:1005";
        //public static string IdentityServerEndPoint = "https://localhost:3345";
        public static string BlogAPI = "Blog.API";
    }
    public class Endpoint
    {
        private IConfiguration _configuration;
        public Endpoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Mvc => _configuration["MVCUri"];
        public string Api => _configuration["APIUri"];
        public string Id4 => _configuration["IdentityServerUri"];
    }
}