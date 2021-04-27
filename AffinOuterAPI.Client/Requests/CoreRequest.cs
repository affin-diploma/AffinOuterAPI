namespace AffinOuterAPI.Client.Requests
{
    public class CoreRequest
    {
        public string query { get; set; } = "";
        public int? page { get; set; } = 1;
        public int? pageSize { get; set; } = 10;
    }
}
