namespace AffinOuterAPI.Client.Models
{
    public class Filter
    {
        public string titles { get; set; }
        public string authors { get; set; }
        public string publishers { get; set; }
        public string languages { get; set; }
        public string years { get; set; }

        public void Deconstruct(out string titles, out string authors, 
            out string publishers, out string languages, out string years)
        {
            titles = this.titles;
            authors = this.authors;
            publishers = this.publishers;
            languages = this.languages;
            years = this.years;
        }
    }
}
