using AffinOuterAPI.Client.Models;
using AffinOuterAPI.Client.Models.Scopus;
using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AffinOuterAPI.BLL.Services
{
    public static class ArticleService
    {
        public static BaseResponse GetArticlesCore(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(BaseRequest)} is null");
            }

            CoreRequest coreRequest = RequestHelper.ToCoreRequest(request);
            if (request.filter != null)
            {
                coreRequest.query = new FilterService(request.filter)
                    .FilterCoreRequest(coreRequest.query);
            }

            BaseResponse articlesResponse = new BaseResponse();
            List<CoreSource> data = new List<CoreSource>();

            int previousDataCount;
            int currentDataCount;
            string responseJson = string.Empty;

            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest);
                responseJson = ApiService.ExecuteCoreApiRequest(queryJson);

                if (responseJson != string.Empty)
                {
                    CoreResponse coreResponse = ResponseHelper.ToCoreResponse(responseJson);

                    previousDataCount = data.Count;
                    if (coreResponse != null)
                    {
                        coreResponse.data = coreResponse.data?.Where(x =>
                            !string.IsNullOrEmpty(x?._source?.downloadUrl) &&
                            !string.IsNullOrEmpty(x?._source?.deleted) && x._source.deleted != "DELETED" &&
                            !string.IsNullOrEmpty(x?._source?.doi))?.ToList();
                    }

                    data.AddRange(coreResponse.data ?? new List<CoreSource>());
                    data = data?.GroupBy(x => x._source.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<CoreSource>();
                    currentDataCount = coreResponse?.data?.Count ?? 0;

                    if (request.limit.Value <= data.Count || currentDataCount == 0)
                    {
                        coreResponse.data = currentDataCount == 0 ? data : data.GetRange(0, request.limit.Value);

                        articlesResponse = ResponseHelper.ToBaseResponse(coreResponse);
                        break;
                    }
                    else coreRequest.page++;
                }
            }
            while (responseJson != string.Empty);

            articlesResponse.request = request;
            return articlesResponse;
        }

        public static BaseResponse GetArticlesCore2(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(BaseRequest)} is null");
            }

            Core2Request coreRequest = RequestHelper.ToCore2Request(request);

            if (request.filter != null)
            {
                coreRequest.basicQuery.searchCriteria = new FilterService(request.filter)
                    .FilterCore2Request(coreRequest.basicQuery.searchCriteria);
            }

            BaseResponse articlesResponse = new BaseResponse();
            List<CoreArticle> data = new List<CoreArticle>();
            int previousDataCount;
            int currentDataCount;
            string responseJson = string.Empty;

            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest, Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                responseJson = ApiService.ExecuteCore2ApiRequest(queryJson);
                if (responseJson != string.Empty)
                {
                    Core2Response coreResponse = ResponseHelper.ToCore2Response(responseJson);

                    previousDataCount = data.Count;
                    if (coreResponse != null)
                    {
                        coreResponse.results = coreResponse.results?.Where(x =>
                            !string.IsNullOrEmpty(x?.downloadUrl))?.ToList();
                    }

                    data.AddRange(coreResponse.results ?? new List<CoreArticle>());
                    data = data?.GroupBy(x => x.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<CoreArticle>();
                    currentDataCount = coreResponse?.results?.Count ?? 0;

                    if (request.limit.Value <= data.Count || currentDataCount == 0)
                    {
                        coreResponse.results = currentDataCount == 0 ? data : data.GetRange(0, request.limit.Value);

                        articlesResponse = ResponseHelper.ToBaseResponse(coreResponse);
                        break;
                    }
                    else coreRequest.basicQuery.offset++;
                }
            }
            while (responseJson != string.Empty);

            articlesResponse.request = request;
            return articlesResponse;
        }

        public static BaseResponse GetArticlesScopus(BaseRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException($"{nameof(BaseRequest)} is null");
            }

            string doi = string.Empty;
            string query = string.Empty;

            ScopusRequest scopusRequest = RequestHelper.ToScopusRequest(request);

            if (request.filter != null)
            {
                scopusRequest.query = new FilterService(request.filter).FilterScopusRequest(scopusRequest.query);
            }
            else scopusRequest.query = $"all({scopusRequest.query})".Replace("(", "%28").Replace(")", "%29").Replace(" ", "+");

            BaseResponse articlesResponse = new BaseResponse();
            List<ScopusArticle> data = new List<ScopusArticle>();

            int previousDataCount;
            int currentDataCount;
            string responseJson = string.Empty;

            do
            {
                string queryJson = $"&start={(scopusRequest.start - 1) * scopusRequest.count}&count={scopusRequest.count}&query={scopusRequest.query}";
                responseJson = ApiService.ExecuteScopusSearchApiRequest(queryJson);

                if (responseJson != string.Empty)
                {
                    ScopusResponse scopusResponse = ResponseHelper.ToScopusResponse(responseJson);

                    previousDataCount = data.Count;
                    if (scopusResponse != null)
                    {
                        scopusResponse.entry = scopusResponse.entry?.Where(x => !string.IsNullOrEmpty(x?.doi))?.ToList();
                    }

                    data.AddRange(scopusResponse?.entry ?? new List<ScopusArticle>());
                    data = data?.GroupBy(x => x.doi)?.Select(x => x.First())?.ToList() ?? new List<ScopusArticle>();
                    currentDataCount = scopusResponse?.entry?.Count ?? 0;

                    if (request.limit.Value <= data.Count || currentDataCount == 0)
                    {
                        scopusResponse.entry = currentDataCount == 0 ? data : data.GetRange(0, request.limit.Value);

                        for (int i = 0; i < scopusResponse.entry.Count; i++)
                        {
                            ScopusArticle article = scopusResponse.entry[i];
                            scopusResponse.entry[i].downloadUrl = ApiService.BuildScopusDownloadRequestUrl(article.doi);
                        }

                        articlesResponse = ResponseHelper.ToBaseResponse(scopusResponse);
                        break;
                    }
                    else scopusRequest.start++;
                }
            }
            while (responseJson != string.Empty);

            articlesResponse.request = request;
            return articlesResponse;
        }
    }
}