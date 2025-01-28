using TomsNewsApi.Dtos;

namespace TomsNewsApi.Services;

public interface IHackerNewsService
{
    public Task<string> GetStoriesAsync();

    public Task<IEnumerable<SearchItem>> GetStoryLinkPage(IEnumerable<string> storyIds, string query);
}
