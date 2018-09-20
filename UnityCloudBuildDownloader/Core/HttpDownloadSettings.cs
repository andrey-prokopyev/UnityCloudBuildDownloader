using System;

namespace UnityCloudBuildDownloader.Core
{
    public class HttpDownloadSettings
    {
        public string AuthKey { get; set; }

        public int PageSize { get; set; }

        public Uri BaseUri { get; set; }

        public Uri GetBuildsUri { get; set; }
    }
}