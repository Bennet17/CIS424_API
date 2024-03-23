using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

public class BaseApiController : ApiController
{
    protected const string SharedKey = "password";

    protected bool AuthenticateRequest(HttpRequestMessage request)
    {
        if (request.Headers.TryGetValues("API_Key", out var headerValues))
        {
            var apiKey = headerValues.FirstOrDefault();
            if (apiKey == SharedKey)
            {
                return true;
            }
        }
        return false;
    }
}