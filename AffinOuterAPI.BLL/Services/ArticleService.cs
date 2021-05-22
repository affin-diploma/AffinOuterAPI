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
            do
            {
                string queryJson = JsonConvert.SerializeObject(coreRequest);
                string responseJson = ApiService.ExecuteCoreApiRequest(queryJson);

                if (responseJson != string.Empty)
                {
                    CoreResponse coreResponse = ResponseHelper.ToCoreResponse(responseJson);

                    previousDataCount = data.Count;
                    if(coreResponse != null)
                    {
                        coreResponse.data = coreResponse.data?.Where(x => !string.IsNullOrEmpty(x?._source?.downloadUrl) && 
                            (string.IsNullOrEmpty(x?._source?.deleted) ||  
                                x._source.deleted != "DELETED"))?.ToList();
                    }
                    data.AddRange(coreResponse.data ?? new List<CoreSource>());
                    data = data?.GroupBy(x => x._source.downloadUrl)?.Select(x => x.First())?.ToList() ?? new List<CoreSource>();
                    currentDataCount = coreResponse?.data?.Count ?? 0;
                    if (coreRequest.pageSize.Value <= data.Count || currentDataCount == 0)
                    {
                        if (currentDataCount == 0)
                        {
                            coreResponse.data = data;
                        }
                        else
                        {
                            coreResponse.data = data.GetRange(0, coreRequest.pageSize.Value);
                        }
                        articlesResponse = ResponseHelper.ToBaseResponse(coreResponse);
                        break;
                    }
                    else coreRequest.page++;
                }
            }
            while (true);

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

            BaseResponse articlesResponse = new BaseResponse();
            List<ScopusArticle> data = new List<ScopusArticle>();
            int previousDataCount;
            int currentDataCount;
            do
            {
                scopusRequest.query = $"all({scopusRequest.query})"
                    .Replace("(", "%28")
                    .Replace(")", "%29")
                    .Replace(" ", "+");

                string queryJson = $"&start={(scopusRequest.start - 1) * scopusRequest.count}&count={scopusRequest.count}&query={scopusRequest.query}";
                string responseJson = ApiService.ExecuteScopusSearchApiRequest(queryJson);

                if (responseJson != string.Empty)
                {
                    ScopusResponse scopusResponse = ResponseHelper.ToScopusResponse(responseJson);

                    previousDataCount = data.Count;
                    if(scopusResponse != null)
                    {
                        scopusResponse.entry = scopusResponse.entry?.Where(x => !string.IsNullOrEmpty(x?.doi))?.ToList();
                    }
                    data.AddRange(scopusResponse?.entry ?? new List<ScopusArticle>());
                    data = data?.GroupBy(x => x.doi)?.Select(x => x.First())?.ToList() ?? new List<ScopusArticle>();
                    currentDataCount = scopusResponse?.entry?.Count ?? 0;
                    if (scopusRequest.count.Value <= data.Count || currentDataCount == 0)
                    {
                        if (currentDataCount == 0)
                        {
                            scopusResponse.entry = data;
                        }
                        else
                        {
                            scopusResponse.entry = data.GetRange(0, scopusRequest.count.Value);
                        }

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
            while (true);

            articlesResponse.request = request;
            return articlesResponse;
        }
    }
}