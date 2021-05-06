using AffinOuterAPI.Client.Models;
using System.Collections.Generic;
using System.Linq;

namespace AffinOuterAPI.BLL.Services
{
    public class FilterService
    {
        private Filter _currentFilter { get; set; }
        public FilterService(Filter filter)
        {
            _currentFilter = filter;
        }

        public string FilterCoreRequest(string coreRequest)
        {
            if (string.IsNullOrEmpty(coreRequest) || _currentFilter == null) return string.Empty;

            List<string> filterQuery = new List<string>();

            if(!string.IsNullOrEmpty(_currentFilter.title))
            {
                List<string> titleFilter = new List<string>();
                List<string> titles = _currentFilter.title.Split(";").ToList();
                foreach (string title in titles)
                {
                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title.Contains(" "))
                        {
                            titleFilter.Add($"\"{title}\"");
                        }
                        else
                        {
                            titleFilter.Add(title);
                        }
                    }
                }

                filterQuery.Add($"title:({string.Join(" OR ", titleFilter)})");
            }

            if(!string.IsNullOrEmpty(_currentFilter.year))
            {
                filterQuery.Add($"year:{_currentFilter.year}");
            }

            /// OTHER FILTERS HERE
            ///--
            ///--
            
            if(filterQuery.Any())
            {
                coreRequest += $" {string.Join(" AND ", filterQuery)}";
            }

            return coreRequest;
        }
    }
}
