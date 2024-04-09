using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CIS424_API.Controllers
{
    // These 2 routes will just be to test if the API is active and available.
    // It'll authenticate the user and return something if they are missing the api key
    // One route is a post request, one is a get request.
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : BaseApiController
    {

        [HttpGet]
        [Route("Test")]
        public IHttpActionResult GetHelloWorld()
        {
            if (!AuthenticateRequest(Request))
            {
                //Return unauthorized response with custom message
                return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
            }
            return Ok("Hello, FrontEnd Team!");
        }

        [HttpPost]
        [Route("Test")]
        public IHttpActionResult PostHelloWorld()
        {
            if (!AuthenticateRequest(Request))
            {
                //Return unauthorized response with custom message
                return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
            }
            return Ok("Hello, FrontEnd Team!");
        }
    }
}
