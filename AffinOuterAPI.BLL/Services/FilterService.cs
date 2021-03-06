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

        private bool GetCoreFilterQuery(string codeName, string criterias, out string filterQuery)
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
                        criteriasFilter.Add($"({string.Join(" AND ", criteria.Split('&').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Contains(" ") ? $"\"{x}\"" : x))})");
                    }
                    else criteriasFilter.Add(criteria.Contains(" ") ? $"\"{criteria}\"" : criteria);
                }

                string joinedFilter = string.Join(" OR ", criteriasFilter.Where(x => !string.IsNullOrEmpty(x)));
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}:({joinedFilter})";
                }
            }
            else if (criterias.Contains("&"))
            {
                string joinedFilter = string.Join(" AND ", criterias.Split('&').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Contains(" ") ? $"\"{x}\"" : x));
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}:({joinedFilter})";
                }
            }
            else if (criterias.Contains("-"))
            {
                string[] fromTo = criterias.Split("-");
                if (fromTo.Length > 1)
                {
                    filterQuery = $"{codeName}:[{fromTo[0]} TO {fromTo[1]}]";
                }
            }
            else
            {
                string joinedFilter = criterias.Contains(" ") ? $"\"{criterias}\"" : criterias;
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}:({joinedFilter})";
                }
            }

            return !string.IsNullOrEmpty(filterQuery);
        }

        private bool GetScopusFilterQuery(string codeName, string criterias, out string filterQuery)
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
                        criteriasFilter.Add($"( {string.Join(" AND ", criteria.Split('&').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Contains(" ") ? $"\"{x}\"" : x))} )");
                    }
                    else criteriasFilter.Add(criteria.Contains(" ") ? $"\"{criteria}\"" : criteria);
                }

                string joinedFilter = string.Join(" OR ", criteriasFilter.Where(x => !string.IsNullOrEmpty(x)));
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}( {joinedFilter} )";
                }
            }
            else if (criterias.Contains("&"))
            {
                string joinedFilter = string.Join(" AND ", criterias.Split('&').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Contains(" ") ? $"\"{x}\"" : x));
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}( {joinedFilter} )";
                }
            }
            else if (criterias.Contains("-"))
            {
                string[] fromTo = criterias.Split("-");
                if (fromTo.Length > 1)
                {
                    filterQuery = $"{codeName}( > {fromTo[0]} AND < {fromTo[1]} )";
                }
            }
            else
            {
                string joinedFilter = criterias.Contains(" ") ? $"\"{criterias}\"" : criterias;
                if (!string.IsNullOrEmpty(joinedFilter))
                {
                    filterQuery = $"{codeName}( {joinedFilter} )";
                }
            }

            return !string.IsNullOrEmpty(filterQuery);
        }

        public string FilterCoreRequest(string coreRequest)
        {
            if (string.IsNullOrEmpty(coreRequest) || _currentFilter == null) return coreRequest;
            (string titles, string topics, string authors,
                string publishers, string repositories, string languages, string years) = _currentFilter;

            titles = string.IsNullOrEmpty(titles) ? coreRequest : $"{coreRequest}|{titles}";
            List<string> filterQuery = new List<string>();
            foreach (KeyValuePair<string, string> codeCriteria in new Dictionary<string, string>
                {
                    {"title", titles},
                    {"topics", topics},
                    {"authors", authors},
                    {"publisher", publishers},
                    {"language", languages},
                    {"year", years}
                })
            {
                if (!string.IsNullOrEmpty(codeCriteria.Key) &&
                    !string.IsNullOrEmpty(codeCriteria.Value) &&
                    GetCoreFilterQuery(codeCriteria.Key, codeCriteria.Value, out string filter))
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

        public string FilterCore2Request(string coreRequest)
        {
            if (string.IsNullOrEmpty(coreRequest) || _currentFilter == null) return coreRequest;
            (string titles, string topics, string authors,
                string publishers, string repositories, string languages, string years) = _currentFilter;

            List<string> filterQuery = new List<string>();
            foreach (KeyValuePair<string, string> codeCriteria in new Dictionary<string, string>
                {
                    {"year", years},
                    {"author", authors},
                    {"publisher", publishers},
                    {"repository", repositories},
                })
            {
                if (!string.IsNullOrEmpty(codeCriteria.Key) &&
                    !string.IsNullOrEmpty(codeCriteria.Value) &&
                    GetCoreFilterQuery(codeCriteria.Key, codeCriteria.Value, out string filter))
                {
                    filterQuery.Add(filter);
                }
            }

            if (filterQuery.Any())
            {
                coreRequest = $"({coreRequest}) AND {string.Join(" AND ", filterQuery)}";
            }

            return coreRequest;
        }

        public string FilterScopusRequest(string coreRequest)
        {
            if (string.IsNullOrEmpty(coreRequest) || _currentFilter == null) return coreRequest;
            (string titles, string topics, string authors,
                 string publishers, string repositories, string languages, string years) = _currentFilter;

            titles = string.IsNullOrEmpty(titles) ? coreRequest : $"{coreRequest}|{titles}";
            List<string> filterQuery = new List<string>();
            foreach (KeyValuePair<string, string> codeCriteria in new Dictionary<string, string>
                {
                    {"all", titles},
                    {"key", $"{topics}|{publishers}|{repositories}"},
                    {"aut", authors},
                    {"abs", languages},
                    {"pub-date", years}
                })
            {
                if (!string.IsNullOrEmpty(codeCriteria.Key) &&
                    !string.IsNullOrEmpty(codeCriteria.Value) &&
                    GetScopusFilterQuery(codeCriteria.Key, codeCriteria.Value, out string filter))
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
