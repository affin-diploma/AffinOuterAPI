using AffinOuterAPI.Client.Models.Scopus;

namespace AffinOuterAPI.Client.Models
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

        public static Article ToArticle(CoreArticle obj)
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
                lang = obj?.language?.name,
                year = obj?.year,
                relations = obj?.relations,
            };
        }

        public static Article ToArticle(ScopusArticle obj)
        {
            return new Article
            {
                id = obj?.id,
                title = obj?.title,
                authors = obj?.authors,
                publisher = obj?.publisher,
                downloadUrl = obj?.downloadUrl,
                year = obj?.year?.Year
            };
        }
    }
}