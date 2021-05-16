namespace AffinOuterAPI.BLL.Services
{
    public static class ModuleService
    {
        // Core 
        public const string CoreApiUrl = "https://core.ac.uk:443/api-v2/search";
        public const string CoreApiKey = "EhozeN2gOrCRJd6I1AFD7kUHwPYa4ZQm";

        // Scopus
        private const string ScopusBaseApiUrl = "https://api.elsevier.com/content";
        public const string ScopusSearchApiKey = "7f59af901d2d86f78a1fd60c1bf9426a";
        public const string ScopusDownloadApiKey = "2bf97398f47f6e442485c3705e789b2a";
        public static string scopusSearchApiUrl = $"{ScopusBaseApiUrl}/search/sciencedirect?{{query}}&apiKey={ScopusSearchApiKey}";
        public static string scopusDonwloadApiUrl = $"{ScopusBaseApiUrl}/article/doi/{{doi}}?apiKey={ScopusDownloadApiKey}&httpAccept=application%2Fpdf";
    }
}
