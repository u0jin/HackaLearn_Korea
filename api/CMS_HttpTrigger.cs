using System;
using System.Net.Http;

using api.Models;

using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace api
{
    public static class CMS_HttpTrigger
    {
        [FunctionName("CMS_HttpTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var blogUri = Environment.GetEnvironmentVariable("BLOG_URI");
            var requestUri = $"https://public-api.wordpress.com/rest/v1.1/sites/{blogUri}/posts/";
            var reponse = default(PostCollection);

            using (var http = new HttpClient())
            {
                var serialised = await http.GetStringAsync(requestUri);
                reponse = JsonConvert.DeserializeObject<PostCollection>(serialised);
            }

            return new OkObjectResult(reponse);
        }
    }
}