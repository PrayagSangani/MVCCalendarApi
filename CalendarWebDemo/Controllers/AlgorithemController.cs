using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CalendarWebDemo.Controllers
{
    public class AlgorithemController : Controller
    {
        // GET: Algorithem
        public ActionResult Index()
        {
            //int SumOfResult=SumTwoNumber(10,20);
            string paragraph = "Bob hit a ball, the hit BALL flew far after it was hit.";
            List<string> banned =new List<string>() { "hit"};
            string commonworkd = MostCommonWord(paragraph, banned);
            return View();
        }
        public int SumTwoNumber(int A, int B)
        {
            return A + B;
        }
        public string MostCommonWord(string paragraph, List<string> banned)
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