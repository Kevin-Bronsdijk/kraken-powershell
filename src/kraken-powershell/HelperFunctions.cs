using System;
using System.IO;
using Kraken.Model;
using System.Collections.Generic;

namespace Kraken.Powershell
{
    public static class HelperFunctions
    {
        public static string BuildPathAzure(string url, bool keepPath, string path, string container)
        {
            var uri = new Uri(url);

            if (keepPath)
            {
                if (DestinationUsesSameAzureContainer(uri, container))
                {
                    var location = uri.LocalPath.Remove(0, container.Length + 1);
                    return UrlCleanUp($"{location}");
                }

                return UrlCleanUp($"{uri.LocalPath}");
            }

            return UrlCleanUp($"/{path}/{Path.GetFileName(uri.LocalPath)}");
        }

        public static string BuildPathS3(string url, bool keepPath, string path, string bucket)
        {
            var uri = new Uri(url);

            if (keepPath)
            {
                return UrlCleanUp($"{uri.LocalPath.TrimStart('/')}");
            }

            if (string.IsNullOrEmpty(path))
            {
                return UrlCleanUp($"{Path.GetFileName(uri.LocalPath)}");
            }
            else
            {
                return UrlCleanUp($"{path}/{Path.GetFileName(uri.LocalPath)}");
            }
        }

        public static bool DestinationUsesSameAzureContainer(Uri url, string azureContainer)
        {
            return url.AbsoluteUri.ToLower().Contains(Consts.AzureBlobUrl) && url.LocalPath.StartsWith("/" + azureContainer);
        }

        public static string UrlCleanUp(string url)
        {
            return url.Replace("//", "/");
        }

        internal static SamplingScheme ConvertSamplingScheme(string samplingScheme)
        {
            if (samplingScheme == "4:2:2")
            {
                return Kraken.Model.SamplingScheme.S422;
            }
            if (samplingScheme == "4:4:4")
            {
                return Kraken.Model.SamplingScheme.S444;
            }
            else
            {
                return Kraken.Model.SamplingScheme.Default;
            }
        }

        internal static IEnumerable<OptimizeImageItem> CreateOptimizeImageItems(string[] paths)
        {
            foreach (var path in paths)
            {
                yield return new OptimizeImageItem()
                {
                    Path = path
                };
            }
        }
    }
}