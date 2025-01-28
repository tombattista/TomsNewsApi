namespace TomsNewsApi.Dtos;

public class SearchItem
{
    public Guid Id { get; private set; }
    public string Query {  get; set; } = "";
    public List<NewsItem> Items { get; set; } = [];

    public SearchItem()
    {
        Id = Guid.NewGuid();
    }

}
