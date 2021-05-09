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

        private bool GetFilterQuery(string codeName, string criterias, out string filterQuery)
        {
            filterQuery = string.Empty;
         
            if (criterias.Contains("|"))
            {
                List<string> criteriasFilter = new List<string>();
                List<string> splittedCriterias = criterias.Split('|').ToList();

                foreach (string criteria in splittedCriterias)
                {
                    if (criteria.Contains("&"))
                    {
                        criteriasFilter.Add($"({string.Join(" AND ", criteria.Split('&').Select(x => x.Contains(" ") ? $"\"{x}\"" : x))})");
                    }
                    else criteriasFilter.Add(criteria.Contains(" ") ? $"\"{criteria}\"" : criteria);
                }
                filterQuery = $"{codeName}:({string.Join(" OR ", criteriasFilter)})";
            }
            else if (criterias.Contains("&"))
            {
                filterQuery = $"{codeName}:({string.Join(" AND ", criterias.Split('&').Select(x => x.Contains(" ") ? $"\"{x}\"" : x))})";
            }
            else
            {
                filterQuery = $"{codeName}:({(criterias.Contains(" ") ? $"\"{criterias}\"" : criterias)})";
            }
            
            return !string.IsNullOrEmpty(filterQuery);
        }

        public string FilterCoreRequest(string coreRequest)
        {
            if (string.IsNullOrEmpty(coreRequest) || _currentFilter == null) return coreRequest;
            (string titles, string authors,
                string publishers, string languages, string years) = _currentFilter;

            titles = string.IsNullOrEmpty(titles) ? coreRequest : $"{coreRequest}|{titles}";
            List<string> filterQuery = new List<string>();
            foreach (KeyValuePair<string, string> codeCriteria in new Dictionary<string, string>
            {
                {"title", titles},
                {"authors", authors},
                {"publisher", publishers},
                {"language.name", languages},
                {"year", years}
            })
            {
                if (!string.IsNullOrEmpty(codeCriteria.Key) &&
                    !string.IsNullOrEmpty(codeCriteria.Value) &&
                    GetFilterQuery(codeCriteria.Key, codeCriteria.Value, out string filter))
                {
                    filterQuery.Add(filter);
                }
            }

            if (filterQuery.Any())
            {
                coreRequest = string.Join(" AND ", filterQuery);
            }

            return coreRequest;
        }
    }
}
