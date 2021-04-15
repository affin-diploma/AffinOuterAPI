using System;
using System.Collections.Generic;
using System.Text;

namespace AffinOuterAPI.Client.Requests
{
    public class GetArticlesRequest
    {
        public int offset { get; set; }
        public int count { get; set; }
        public string searchQuery { get; set; }
    }
}
