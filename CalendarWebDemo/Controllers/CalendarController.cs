using Code7248.word_reader;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CalendarWebDemo.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        string UserEmail = "prayagmicro@gmail.com";
        public ActionResult Index()     
        {

            CalendarController cs = new CalendarController();
            string path = @"D:\prayag\testsave.docx";
            cs.readFileContent(path);
            Console.ReadLine();

            string _JsonFilePath = Path.Combine(Server.MapPath("~/App_Data"), "client_secret_360228937755.json");
            CalendarService _service = this.GetCalendarService(_JsonFilePath, UserEmail);

            CreateEvent(_service,  UserEmail);
           // DeleteEvents(_service);
           // GetEvents(_service);
            return View();
        }
        private void readFileContent(string path)
        {
            TextExtractor extractor = new TextExtractor(path);
            string text = extractor.ExtractText();
            Console.WriteLine(text);
        }
        private CalendarService GetCalendarService(string keyfilepath, string UserEmail)
        {
            try
            {
                string[] Scopes = {CalendarService.Scope.Calendar,
                   CalendarService.Scope.CalendarEvents
                   /*CalendarService.Scope.CalendarEventsReadonly*/ };
                string ApplicationName = "Extendrum Api";

                UserCredential credential;

                using (var stream =
                    new FileStream(keyfilepath, FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath =  Path.Combine(Server.MapPath("~/App_Data"),"Googletoken.json");
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        UserEmail,
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
                return service;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void CreateEvent(CalendarService _service, string UserEmail)
        {
            Event body = new Event();
            EventAttendee a = new EventAttendee();
            a.Email = "atumaischeduling@gmail.com";
            List<EventAttendee> attendes = new List<EventAttendee>();
            attendes.Add(a);
            body.Attendees = attendes;
            EventDateTime start = new EventDateTime();
            string iString = "2020-05-30 06:12 PM";
            DateTime oDate = DateTime.ParseExact(iString, "yyyy-MM-dd hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            start.DateTime = oDate;

            EventDateTime end = new EventDateTime();
            string ieString = "2020-05-30 08:12 PM";
            DateTime oeDate = DateTime.ParseExact(ieString, "yyyy-MM-dd hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            end.DateTime = oeDate;

            body.Start = start;
            body.End = end;
            body.Location = "Newberg, OR 97132, USA";
            body.Summary = "Video ";
            // body.Description = "prayag";
            EventsResource.InsertRequest request = new EventsResource.InsertRequest(_service, body, UserEmail);
            Event response = request.Execute();
        }
        private void DeleteEvents(CalendarService _service)
        {
            string CalendarId = "";
            EventsResource.DeleteRequest Delrequest = new EventsResource.DeleteRequest(_service, UserEmail, CalendarId);
            Delrequest.Execute();
        }
        private void GetEvents(CalendarService _service)
        {
            // Define parameters of request.    
            EventsResource.ListRequest request = _service.Events.List("primary");
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 10;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            string eventsValue = "";
            // List events.    
            Events events = request.Execute();
            eventsValue = "Upcoming events:\n";
            if (events.Items != null && events.Items.Count > 0)
            {
                foreach (var eventItem in events.Items)
                {
                    string when = eventItem.Start.DateTime.ToString();
                    if (String.IsNullOrEmpty(when))
                    {
                        when = eventItem.Start.Date;
                    }
                    eventsValue += string.Format("{0} ({1})", eventItem.Summary, when) + "\n";
                }
            }
        }

        public string MostCommonWord(string paragraph, string[] banned)
        {
            string result = string.Empty;
            int max = Int32.MinValue;
            HashSet<string> bannedWords = new HashSet<string>();
            Dictionary<string, int> dict = new Dictionary<string, int>();

            paragraph = paragraph.Replace("!", " ")
                                 .Replace("?", " ")
                                 .Replace("'", " ")
                                 .Replace(",", " ")
                                 .Replace(";", " ")
                                 .Replace(".", " ")
                                 .Trim();

            foreach (var item in banned)
                bannedWords.Add(item.ToLower());

            foreach (var item in paragraph.Split(' '))
                if (item != string.Empty && !bannedWords.Contains(item.ToLower()))
                {
                    if (!dict.ContainsKey(item.ToLower()))
                        dict.Add(item.ToLower(), 0);

                    dict[item.ToLower()] += 1;
                }

            foreach (var item in dict.Keys)
                if (dict[item] > max)
                {
                    result = item;
                    max = dict[item];
                }

            return result;
        }
    }
}