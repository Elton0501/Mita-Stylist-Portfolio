using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Photography.Web.Controllers
{
    public class webApiController : ApiController
    {
        [HttpGet]
        public string GetTestResult()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string testres = "start";
                //string Rip = HttpContext.Current.Session["ApiIP"].ToString();
                //IEnumerable<Menu> MenuModel = null;
                using (var client = new HttpClient())
                {
                    testres = "pending";
                    client.BaseAddress = new Uri("http://103.69.114.237:8080");
                    //HTTP GET
                    var url = "http://103.69.114.237:8080/api/user/MENU";
                    //Uri myUri = new Uri(url, UriKind.Absolute);
                    var responseTask = client.GetAsync(url);
                    responseTask.Wait();
                    testres = "response waiting";
                    var result = responseTask.Result;
                    testres = "response resulted";
                    if (result.IsSuccessStatusCode)
                    {
                        //testres = result.StatusCode.ToString();
                        testres = "true";
                    }
                    else //web api sent error response 
                    {
                        //log response status here..
                        testres = "false";
                    }
                }
                return testres;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
