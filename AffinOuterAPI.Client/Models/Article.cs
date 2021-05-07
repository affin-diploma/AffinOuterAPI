﻿namespace AffinOuterAPI.Client.Models
{
    public class Article : BaseArticle
    {
        public string lang { get; set; }
    }

    public static class ArticleHelper
    {
        public static Article ToArticle<T>(T obj) where T : BaseArticle
        {
            return new Article
            {
                id = obj?.id,
                title = obj?.title,
                topics = obj?.topics,
                authors = obj?.authors,
                publisher = obj?.publisher,
                downloadUrl = obj?.downloadUrl,
                description = obj?.description,
                year = obj?.year,
                relations = obj?.relations,
            };
        }

        public static Article ToArticleFromCore<T>(T obj) where T : CoreArticle
        {
            return new Article
            {
                id = obj?.id,
                title = obj?.title,
                topics = obj?.topics,
                authors = obj?.authors,
                publisher = obj?.publisher,
                downloadUrl = obj?.downloadUrl,
                description = obj?.description,
                lang = obj?.language?.code,
                year = obj?.year,
                relations = obj?.relations,
            };
        }
    }
}