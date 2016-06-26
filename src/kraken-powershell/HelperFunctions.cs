using System;
using System.IO;

namespace Kraken.Powershell
{
    public static class HelperFunctions
    {
        public static string BuildPath(string url, bool keepPath, string path, string root)
        {
            // root is a container or bucket
            var uri = new Uri(url);

            if (keepPath)
            {
                //todo: S3
                if (DestinationUsesSameAzureContainer(uri, root))
                {
                    var location = uri.LocalPath.Remove(0, root.Length + 1);
                    return UrlCleanUp($"{path}{location}");
                }

                return UrlCleanUp($"{path}{uri.LocalPath}");
            }

            return UrlCleanUp($"/{path}/{Path.GetFileName(uri.LocalPath)}");
        }

        public static bool DestinationUsesSameAzureContainer(Uri url, string azureContainer)
        {
            return url.AbsoluteUri.ToLower().Contains("blob.core.windows.net") && url.LocalPath.StartsWith("/" + azureContainer);
        }

        public static string UrlCleanUp(string url)
        {
            return url.Replace("//", "/");
        }
    }
}