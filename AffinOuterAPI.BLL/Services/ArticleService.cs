using AffinOuterAPI.Client.Requests;
using AffinOuterAPI.Client.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AffinOuterAPI.BLL.Services
{
    public class ArticleService
    {
        public GetArticlesResponse GetArticles(GetArticlesRequest articles)
        {
            if(articles == null)
            {
                throw new ArgumentException($"{nameof(GetArticlesRequest)} is null");
            }

            // HERE NEED TO BE IMPLEMENTATION OF GET REQUEST!!!
            return new GetArticlesResponse();
        }
    }
}
