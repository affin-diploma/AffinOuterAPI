using System.Collections.Generic;

namespace AffinOuterAPI.Client.Models
{
    public class BaseArticle
    {
        public string doi { get; set; }
        public string title { get; set; }
        public List<string> topics { get; set; }
        public string description { get; set; }
        public List<string> authors { get; set; }
        public string publisher { get; set; }
        public string downloadUrl { get; set; }
        public string deleted { get; set; }
        public int? year { get; set; }
        public List<string> relations { get; set; }
        public string source { get; set; }
    }
}
