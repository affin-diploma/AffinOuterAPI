using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace AffinOuterAPI.Client.Requests
{
    public class GetArticlesRequest
    {
        public int offset { get; set; }
        public int count { get; set; }
        public string searchQuery { get; set; }
    }
}
