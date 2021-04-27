using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AffinOuterAPI.Client.Models
{
    public class Filter
    {
        public string title { get; set; }
        public List<string> topics { get; set; }
        public string authors { get; set; }
        public string publisher { get; set; }
        public string language { get; set; }
        public string year { get; set; }

        [JsonIgnore]
        public DateTime? startYear { get; set; }
        [JsonIgnore]
        public DateTime? endYear { get; set; }
    }
}
