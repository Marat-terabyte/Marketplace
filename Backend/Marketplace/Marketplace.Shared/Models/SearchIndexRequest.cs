namespace Marketplace.Shared.Models
{
    public class SearchIndexRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public RequestType Type { get; set; }
    }

    public enum RequestType
    {
        Index, Deindex, Update
    }
}
