using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using TomsNewsApi.Dtos;
using TomsNewsApi.Services;

namespace TomsNewsApi.Controllers
{
    [EnableCors("AllowLocalhost4200")]
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController(HackerNewsService newsService, ILogger<StoriesController> logger) : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger = logger;
        private readonly HackerNewsService _newsService = newsService;

        /// <summary>
        /// GET: api/<StoriesController>
        /// </summary>
        /// <returns>Collection of stories, based on input query</returns>
        [HttpGet("search")]
        public async Task<IEnumerable<SearchItem>> Get([FromQuery] string query)
        {
            _logger.LogInformation($"Get: {query}");

            string storyList = await _newsService.GetStoriesAsync();
            List<string>? storyIds = JsonConvert.DeserializeObject<List<string>>(storyList);

            if (storyIds is null)
            {
                return [];
            }

            List<SearchItem> searchResults = (List<SearchItem>) await _newsService.GetStoryLinkPage(storyIds, query);

            return searchResults;

            /*
            return
            [
                new SearchItem() { Query = query, Items = [
                    new NewsItem() { Title = "Google", Link = "https://www.google.com" },
                    new NewsItem() { Title = "Angular Material Icons", Link = "https://www.angularjswiki.com/angular/angular-material-icons-list-mat-icon-list/#angular-material-icons-list-categories" },
                    new NewsItem() { Title = "LinkedIn - Tom Battista", Link = "https://www.linkedin.com/in/tom-battista-6bb56716/" },
                    new NewsItem() { Title = "Sensation News Item 5", Link = "https://www.yahoo.com" },
                    new NewsItem() { Title = "LinkedIn Feed", Link = "https://www.linkedin.com/feed/?highlightedUpdateType=SHARED_BY_YOUR_NETWORK&highlightedUpdateUrn=urn%3Ali%3Aactivity%3A7288966182869602304" },
                    new NewsItem() { Title = "Microsoft Network", Link = "https://www.msn.com" },
                    new NewsItem() { Title = "NVidia - Video Encode and Decode", Link = "https://developer.nvidia.com/video-encode-and-decode-gpu-support-matrix-new" },
                    new NewsItem() { Title = "Michigan Tech News - Quantum Teleportation Achieved Over Internet", Link = "https://mitechnews.com/science/quantum-teleportation-achieved-over-internet/" },
                    new NewsItem() { Title = "Nextech", Link = "https://www.nextech.com/" },
                    new NewsItem() { Title = "Nextech AI Solutions", Link = "https://www.nextech.com/solutions/ai" },
                    new NewsItem() { Title = "Nextech on Glassdoor", Link = "https://www.glassdoor.com/Reviews/Nextech-Systems-Reviews-E557514.htm" },
                    new NewsItem() { Title = "Nextech Bing Search", Link = "https://www.bing.com/search?pglt=425&q=nextech+systems+company+reviews&cvid=8dcfbed2738a493080c62434d03afe2c&gs_lcrp=EgRlZGdlKgcIABAAGPkHMgcIABAAGPkHMgYIARAAGEAyBggCEEUYOTIGCAMQABhAMgYIBBAAGEAyBggFEAAYQDIGCAYQABhAMgYIBxBFGD0yBggIEEUYPdIBCDE2ODhqMGoxqAIAsAIA&FORM=ANNTA1&PC=U531" }
                ]},
                new SearchItem() { Query = string.Join(' ', query.Split(" ").Take(2)), Items = [
                    new NewsItem() { Title = "Google", Link = "https://www.google.com" },
                    new NewsItem() { Title = "Angular Material Icons", Link = "https://www.angularjswiki.com/angular/angular-material-icons-list-mat-icon-list/#angular-material-icons-list-categories" },
                    new NewsItem() { Title = "LinkedIn - Tom Battista", Link = "https://www.linkedin.com/in/tom-battista-6bb56716/" },
                    new NewsItem() { Title = "Sensation News Item 5", Link = "https://www.yahoo.com" },
                    new NewsItem() { Title = "LinkedIn Feed", Link = "https://www.linkedin.com/feed/?highlightedUpdateType=SHARED_BY_YOUR_NETWORK&highlightedUpdateUrn=urn%3Ali%3Aactivity%3A7288966182869602304" },
                    new NewsItem() { Title = "Microsoft Network", Link = "https://www.msn.com" },
                    new NewsItem() { Title = "Nextech Bing Search", Link = "https://www.bing.com/search?pglt=425&q=nextech+systems+company+reviews&cvid=8dcfbed2738a493080c62434d03afe2c&gs_lcrp=EgRlZGdlKgcIABAAGPkHMgcIABAAGPkHMgYIARAAGEAyBggCEEUYOTIGCAMQABhAMgYIBBAAGEAyBggFEAAYQDIGCAYQABhAMgYIBxBFGD0yBggIEEUYPdIBCDE2ODhqMGoxqAIAsAIA&FORM=ANNTA1&PC=U531" }
                ]},
            ];
            */
        }
    }
}
