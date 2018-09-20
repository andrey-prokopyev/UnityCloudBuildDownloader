using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serializers;

namespace UnityCloudBuildDownloader.Core
{
    public class HttpBuildDownloader : IBuildDownloader
    {
        private readonly HttpDownloadSettings _httpDownloadSettings;

        private readonly RestClient _client;

        public HttpBuildDownloader(HttpDownloadSettings httpDownloadSettings)
        {
            _httpDownloadSettings = httpDownloadSettings;
            _client = new RestClient(_httpDownloadSettings.BaseUri);
            _client.AddHandler("application/json", new JsonNetSerializer());
        }

        public async Task<IEnumerable<BuildInfo>> GetBuilds(string platform)
        {
            var response = await _client.ExecuteGetTaskAsync<BuildResponse[]>(CreateGetBuildsRequest(platform));
            return response.Data.Select(Map);
        }

        private static BuildInfo Map(BuildResponse response)
        {
            return new BuildInfo
            {
                Version = response.Build,
                Platform = response.Platform,
                Status = response.BuildStatus,
                Changes = response.ChangeSet.Select(Map),
                PackageUrl = response.Links?.DownloadPrimary?.Href
            };
        }

        private static BuildInfo.Change Map(BuildResponse.Change change)
        {
            if (change == null)
            {
                return null;
            }

            return new BuildInfo.Change
            {
                Author = change.Author?.FullName,
                Message = change.Message,
                Timestamp = change.Timestamp
            };
        }

        private IRestRequest CreateGetBuildsRequest(string platform)
        {
            var request = new RestRequest(_httpDownloadSettings.GetBuildsUri, Method.GET);
            request.AddHeader("Authorization", $"Basic {_httpDownloadSettings.AuthKey}");

            request.AddQueryParameter("platform", platform);
            request.AddQueryParameter("page", "1");
            request.AddQueryParameter("per_page", _httpDownloadSettings.PageSize.ToString());

            request.JsonSerializer = new JsonSerializer();
            return request;
        }
    }
}