using System;
using System.Collections.Generic;

namespace UnityCloudBuildDownloader.Core
{
    public class BuildInfo
    {
        public string Version { get; set; }

        public string Status { get; set; }

        public string Platform { get; set; }

        public string PackageUrl { get; set; }

        public IEnumerable<Change> Changes { get; set; }

        public class Change
        {
            public string Author { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}