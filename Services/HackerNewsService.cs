using Newtonsoft.Json.Linq;
using TomsNewsApi.Dtos;

namespace TomsNewsApi.Services;

public class HackerNewsService(IConfiguration config, HttpClient httpClient, ILogger<HackerNewsService> logger) : IHackerNewsService
{
    private readonly IConfiguration _config = config;
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<HackerNewsService> _logger = logger;

    public async Task<string> GetStoriesAsync()
    {
        string hnListUri = _config?.GetValue<string>("AppSettings:HNListUri") ?? "";
        if (hnListUri.Length == 0)
        {
            _logger.LogError("Missing HNListUri configuration");
            return "";
        }
        string requestUrl = $"{hnListUri}.json?orderBy=\"$priority\"&limitToFirst=200";

        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message);
            return "";
        }
    }

    public async Task<IEnumerable<SearchItem>> GetStoryLinkPage(IEnumerable<string> storyIds, string query, int page, int pageSize)
    {
        List<SearchItem> stories = [];
        List<Task<string>> tasks = [];
        foreach (string storyId in storyIds)
        {
            string requestUrl = $"{_config.GetValue<string>("AppSettings:HNItemUri")}/{storyId}.json";
            tasks.Add(GetResponseAsync(_httpClient, requestUrl));
        }

        string[] responses = await Task.WhenAll(tasks);

        string[] queryWords = query.ToLower().Split(' ');
        SearchItem item = new() { Query = query };
        int itemNumber = 0;
        int firstItemNumber = (page - 1) * pageSize;
        int lastItemNumber = page * pageSize - 1;
        foreach (string response in responses)
        {
            JObject json = JObject.Parse(response);
            bool isStory = json.ContainsKey("type") && json.GetValue("type")?.ToString() == "story";
            bool hasUrl = json.ContainsKey("url") && !string.IsNullOrWhiteSpace(json.GetValue("url")?.ToString());
            string storyTitle = json.GetValue("title")?.ToString().ToLower().Trim() ?? "";
            bool hasValidTitle = !string.IsNullOrWhiteSpace(storyTitle);
            bool isQueryMatch = hasValidTitle && queryWords.Any(word => storyTitle.Contains(word));
            if (isStory && hasUrl && isQueryMatch)
            {
                itemNumber++;
                if (itemNumber >= firstItemNumber && itemNumber <= lastItemNumber)
                {
                    string id = json.GetValue("id")?.ToString() ?? GetDefaultId();
                    string link = json.GetValue("url")?.ToString() ?? "";
                    string title = json.GetValue("title")?.ToString() ?? "";
                    item.Items.Add(new NewsItem() { Id = id, Link = link, Title = title });
                }
            }
        }

        stories.Add(item);

        return stories;
    }

    private static async Task<string> GetResponseAsync(HttpClient client, string url)
    {
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    private static string GetDefaultId()
    {
        Random rnd = new();
        return $"Z{rnd.Next(0, 1000000):D6}";
    }
}
