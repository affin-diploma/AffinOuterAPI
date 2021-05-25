using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AffinOuterAPI.Client.Models.Scopus
{
    [DataContract]
    public class ScopusArticle
    {
        [DataMember(Name = "dc:identifier")]
        public string id { get; set; }
        [DataMember(Name = "dc:title")]
        public string title { get; set; }
        [DataMember(Name = "prism:publicationName")]
        public string topic { get; set; }
        [DataMember(Name = "dc:creator")]
        public string publisher { get; set; }
        [DataMember(Name = "authors")]
        public ScopusArticleAuthor authorsList { get; set; }
        public List<string> authors { get; set; }
        [DataMember(Name = "prism:coverDate")]
        public DateTime? year { get; set; }
        [DataMember(Name = "prism:doi")]
        public string doi { get; set; }
        public string downloadUrl { get; set; }
    }

    [DataContract]
    public class ScopusArticleAuthor
    {
        [DataMember(Name = "author")]
        public object author { get; set; }
    }

    [DataContract]
    public class ScopusArticleAuthorElement
    {
        [DataMember(Name = "$")]
        public string author { get; set; }
    }
}
