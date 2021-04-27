namespace AffinOuterAPI.Client.Models
{
    public class Article : BaseArticle
    {
        public string snippet { get; set; }
        public string lang { get; set; }
        public string url { get; set; }
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
                snippet = obj?.description,
                authors = obj?.authors,
                publisher = obj?.publisher,
                url = obj?.downloadUrl,
                year = obj?.year,
                relations = obj?.relations,
            };
        }

        public static Article ToCoreArticle<T>(T obj) where T : CoreArticle
        {
            return new Article
            {
                id = obj?.id,
                title = obj?.title,
                topics = obj?.topics,
                snippet = obj?.description,
                authors = obj?.authors,
                publisher = obj?.publisher,
                lang = obj?.language?.code,
                url = obj?.downloadUrl,
                year = obj?.year,
                relations = obj?.relations,
            };
        }
    }
}