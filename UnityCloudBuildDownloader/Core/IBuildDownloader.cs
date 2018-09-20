using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnityCloudBuildDownloader.Core
{
    public interface IBuildDownloader
    {
        Task<IEnumerable<BuildInfo>> GetBuilds(string platform);
    }
}