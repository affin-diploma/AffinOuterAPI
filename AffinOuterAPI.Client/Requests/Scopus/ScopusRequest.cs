namespace AffinOuterAPI.Client.Requests
{
    public class ScopusRequest
    {
        public string query { get; set; } = "";
        public int? start { get; set; } = 0;
        public int? count { get; set; } = 1;
    }
}
