using DowntimeSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DowntimeSystem.Utils;

namespace DowntimeSystem.Controllers
{
    public class HttpController : Controller
    {
        // GET: Http
        [HttpGet]
        public ActionResult GetAPI(string basicURL, string url)
        {
            RestClient client = new RestClient(basicURL);
            string result = client.Get(url);
            return Content(result);
        }
        // Post: Http
        [HttpPost]
        public ActionResult PostAPI(string basicURL, string url, string data)
        {
            RestClient client = new RestClient(basicURL);
            string result = client.Post(data, url);
            return Json(result);
        }
        [HttpPost]
        public ActionResult PostWithOutBodyAPI([FromBody] PostRequestModel model)
        {
            RestClient client = new RestClient(model.basicURL);
            string result = client.PostWithOutBody(model.url);
            return Json(result);
        }

        // Put: Http
        public ActionResult PutAPI(string basicURL, string url, string data)
        {
            RestClient client = new RestClient(basicURL);
            string result = client.Put(data, url);
            return Json(result);
        }
        // Delete: Http
        [HttpGet]
        public ActionResult DeleteFromQueryAPI(string basicURL, string url, string data)
        {
            RestClient client = new RestClient(basicURL);
            string result = client.DeleteFromQuery(data, url);
            return Content(result);
        }
    }
    
    
public class PostRequestModel
{
    public string basicURL { get; set; }
    public string url { get; set; }
}

}