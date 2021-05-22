using AffinOuterAPI.Client.Models;
using System.Collections.Generic;

namespace AffinOuterAPI.Client.Responses
{
    public class CoreResponse
    {
        public string status { get; set; }
        public int? totalHits { get; set; }
        public List<CoreSource> data { get; set; }
    }
}
