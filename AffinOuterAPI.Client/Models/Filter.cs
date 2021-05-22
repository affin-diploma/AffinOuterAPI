namespace AffinOuterAPI.Client.Models
{
    public class Filter
    {
        public string titles { get; set; }
        public string topics { get; set; }
        public string authors { get; set; }
        public string publishers { get; set; }
        public string repositories { get; set; }
        public string languages { get; set; }
        public string years { get; set; }

        public void Deconstruct(out string titles, out string topics, out string authors,
            out string publishers, out string repositories, out string languages, out string years)
        {
            titles = this.titles;
            topics = this.topics;
            authors = this.authors;
            publishers = this.publishers;
            repositories = this.repositories;
            languages = this.languages;
            years = this.years;
        }
    }
}
