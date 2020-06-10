using CalendarWebDemo.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CalendarWebDemo.Controllers
{
    public class LinkedInController : Controller
    {
        // GET: LinkedIn
        [RequireHttps]
        public ActionResult Index()
        {
            //var request = (HttpWebRequest)WebRequest.Create("https://www.linkedin.com/oauth/v2/accessToken?grant_type=client_credentials&client_id=86r5ibq7rosr97&client_secret=iGs11czgu4ByFndn");
            //var request = (HttpWebRequest)WebRequest.Create("https://www.linkedin.com/oauth/v2/authorization?client_id=86r5ibq7rosr97&redirect_uri=http://aiatum.clientfeedback360.com&response_type=code&scope=r_liteprofile r_emailaddress w_member_social");
            //var response = (HttpWebResponse)request.GetResponse();
            // var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();   
            return View();
        }
        public ActionResult LinkedINAuth(string code, string state)
        {

            string authUrl = "https://www.linkedin.com/uas/oauth2/accessToken";
             string redirectUrl = "https://localhost:44394//LinkedIn/LinkedINAuth";
            var sign = "grant_type=authorization_code" + "&code=" + code + "&redirect_uri=" + HttpUtility.HtmlEncode(redirectUrl) + "&client_id=86r5ibq7rosr97&client_secret=iGs11czgu4ByFndn";
            // var postData = String.Format("grant_type=authorization_code&code={0}&redirect_uri={1}&client_id={2}&client_secret={3}", code, HttpUtility.HtmlEncode(redirectUrl), apiKey, apiSecret);


            HttpWebRequest webRequest = WebRequest.Create(authUrl + "?" + sign) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";

            Stream dataStream = webRequest.GetRequestStream();

            String postData = String.Empty;
            byte[] postArray = Encoding.ASCII.GetBytes(postData);

            dataStream.Write(postArray, 0, postArray.Length);
            dataStream.Close();

            WebResponse response = webRequest.GetResponse();
            dataStream = response.GetResponseStream();


            StreamReader responseReader = new StreamReader(dataStream);
            String returnVal = responseReader.ReadToEnd().ToString();
            responseReader.Close();
            dataStream.Close();
            response.Close();

            ////Fetch AccessToken
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            LinkedINVM linkedINVM = jsonSerializer.Deserialize<LinkedINVM>(returnVal);

            //Get Profile Details
            LinkedINResVM linkedinresvm = GetProfile(linkedINVM);

            return View(linkedinresvm);


        }
        public LinkedINResVM GetProfile(LinkedINVM linkedinvm)
        {
            var StrUrl = "https://api.linkedin.com/v2/me?oauth2_access_token=" + linkedinvm.access_token;

            //Get Profile Details
            RestClient client = new RestClient(StrUrl);

            RestRequest request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            var content = response.Content;

            HttpContext.Response.Write(content);
            
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
            LinkedINResVM linkedinresvm = jsonSerializer.Deserialize<LinkedINResVM>(content);

            return linkedinresvm;
        }
    }
}