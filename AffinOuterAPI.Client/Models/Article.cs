using System;
using System.Collections.Generic;

namespace AffinOuterAPI.Client.Models
{
    public class Article
    {
        public string id { get; set; }
        public List<string> authors { get; set; }
        public string authorsString { get; set; }
        public DateTime datePublished { get; set; }
        public string publisher { get; set; }
        public List<string> relations { get; set; }
        public List<Repository> repositories { get; set; }
        public Repository repository { get; set; }
        public RepositoryDocument repositoryDocument {get; set;}
        public string title { get; set; }
        public string formattedSize { get; set; }
        public Language language { get; set; }
        public string doi { get; set; }
        public string snippet { get; set; }
        public string oai { get; set; }
        public string downloadUrl { get; set; }
    }
}