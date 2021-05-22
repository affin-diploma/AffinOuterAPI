using AffinOuterAPI.Client.Models;
using System.Collections.Generic;

namespace AffinOuterAPI.Client.Responses
{
    public class Core2Response
    {
        public int? total { get; set; }
        public List<CoreArticle> results { get; set; }
    }
}
