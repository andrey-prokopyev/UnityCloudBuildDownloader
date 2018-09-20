using System;
using System.Runtime.Serialization;

namespace UnityCloudBuildDownloader.Core
{
    [DataContract]
    public class BuildResponse
    {
        [DataMember(Name = "build")] public string Build { get; set; }

        [DataMember(Name = "buildStatus")] public string BuildStatus { get; set; }

        [DataMember(Name = "platform")] public string Platform { get; set; }

        [DataMember(Name = "changeset")] public Change[] ChangeSet { get; set; }

        [DataMember(Name = "links")] public BuildLinks Links { get; set; }

        [DataContract]
        public class Change
        {
            [DataMember(Name = "author")] public AuthorInfo Author { get; set; }

            [DataMember(Name = "commitId")] public string CommitId { get; set; }

            [DataMember(Name = "message")] public string Message { get; set; }

            [DataMember(Name = "timestamp")] public DateTime Timestamp { get; set; }
        }

        [DataContract]
        public class AuthorInfo
        {
            [DataMember(Name = "fullName")] public string FullName { get; set; }
        }

        [DataContract]
        public class BuildLinks
        {
            [DataMember(Name = "download_primary")] public DownloadPrimary DownloadPrimary { get; set; }
        }

        [DataContract]
        public class DownloadPrimary
        {
            [DataMember(Name = "href")] public string Href { get; set; }
        }
    }
}