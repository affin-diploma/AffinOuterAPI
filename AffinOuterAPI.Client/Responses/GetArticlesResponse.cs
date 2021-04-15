using System.Collections.Generic;
using AffinOuterAPI.Client.Models;

namespace AffinOuterAPI.Client.Responses
{
    public class GetArticlesResponse
    {
        public int total { get; set; }
        public int offset { get; set; }
        public bool sort_by_date { get; set; }
        public List<Article> results { get; set; }
        public string search_criteria { get; set; }
    }
}
