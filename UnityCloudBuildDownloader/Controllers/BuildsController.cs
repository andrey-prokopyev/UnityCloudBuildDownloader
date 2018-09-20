using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UnityCloudBuildDownloader.Core;

namespace UnityCloudBuildDownloader.Controllers
{
    public class BuildsController : Controller
    {
        private readonly IBuildDownloader _buildDownloader;
        private readonly IMemoryCache _cache;

        public BuildsController(IBuildDownloader buildDownloader, IMemoryCache cache)
        {
            _buildDownloader = buildDownloader;
            _cache = cache;
        }

        [Route("builds/{platform}")]
        public async Task<IActionResult> GetBuilds(string platform)
        {
            if (!_cache.TryGetValue(platform, out IEnumerable<BuildInfo> builds))
            {
                builds = await _buildDownloader.GetBuilds(platform);
                _cache.Set(platform, builds, TimeSpan.FromMinutes(5));
            }

            return Ok(builds);
        }
    }
}